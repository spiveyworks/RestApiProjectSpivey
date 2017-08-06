using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Visits;
using VisitsRepository;
using GeographyRepository;
using Microsoft.AspNetCore.Http;
using WebApi.Authorization;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    public class UserVisitsController : BaseController
    {
        private readonly ILogger _logger;
        private IVisitsRepository _visitsRepository;
        private IGeographyRepository _geographyRepository;
        private static State[] _statesCache = null;
        private static City[] _citiesCache = null;

        public IVisitsRepository VisitsRepository
        {
            get { return this._visitsRepository; }
        }

        public IGeographyRepository GeographyRepository
        {
            get { return this._geographyRepository; }
        }

        public UserVisitsController(ILogger<GeographyController> logger, IVisitsRepository visitsRepository, IGeographyRepository geographyRepository)
        {
            this._logger = logger;
            this._visitsRepository = visitsRepository;
            this._geographyRepository = geographyRepository;
        }
        
        [HttpGet]
        [Route("user/{user}/visits")]
        public async Task<IActionResult> GetUserVisits(int user)
        {
            IActionResult response = null;
            var skip = 0;
            var take = 1000;

            this._logger.LogInformation(LoggingEvents.GET_USER_VISITS, "Getting user visits for userId={user}", user);

            //Extract skip query string param if it exists
            if (Request != null && Request.Query.Where(item => item.Key == "skip").Count() > 0)
            {
                var valString = Request.Query.Where(item => item.Key == "skip").FirstOrDefault().Value;
                var val = 0;

                if (int.TryParse(valString, out val))
                    if (val >= 0)
                        skip = val;
            }

            //Extract take query string param if it exists
            if (Request != null && Request.Query.Where(item => item.Key == "take").Count() > 0)
            {
                var valString = Request.Query.Where(item => item.Key == "take").FirstOrDefault().Value;
                var val = 0;

                if (int.TryParse(valString, out val))
                    if (val >= 0)
                        take = val;
            }

            var tasks = new List<Task>();
            var visitsTask = this.VisitsRepository.GetVisitsByUserId(user, skip, take);
            tasks.Add(visitsTask);
            var cityCacheTask = this.EnsureCityCacheIsPopulated();
            tasks.Add(cityCacheTask);
            var stateCacheTask = this.EnsureStateCacheIsPopulated();
            tasks.Add(stateCacheTask);

            //Do all these tasks at once and wait for them to complete.
            await Task.WhenAll(tasks.ToArray());

            var visits = visitsTask.Result.ToArray();

            if (visits.Length > 0)
            {
                var visitRepresentations = new List<VisitRepresentation>();

                //Retrieve states for the first time, if they haven't already. These can be cached because
                //the dataset is small and doesn't change often.
                if (_statesCache == null)
                    _statesCache = (await this.GeographyRepository.GetStatesAsync()).ToArray();

                //Retrieve cities for the first time, if they haven't already. These can be cached because
                //the dataset is relatively small and doesn't change often.
                if (_citiesCache == null)
                    _citiesCache = (await this.GeographyRepository.GetCitiesAsync()).ToArray();

                foreach (var visit in visits)
                    visitRepresentations.Add(new VisitRepresentation()
                    {
                        City = _citiesCache.Where(c => c.CityId == visit.CityId).FirstOrDefault().Name,
                        State = _statesCache.Where(s => s.StateId == visit.StateId).FirstOrDefault().Abbreviation,
                        Created = visit.Created,
                        User = visit.User,
                        VisitId = visit.VisitId,
                        Links = new VisitRepresentationLinks()
                        {
                            Self = new Link()
                            {
                                Href = string.Format("/user/{0}/visit/{1}", visit.User, visit.VisitId)
                            }
                        }
                    });

                response = this.Ok(visitRepresentations);
            }
            else
            {
                response = this.NotFound();
            }
            
            return response;
        }

        [HttpGet]
        [Route("user/{user}/visits/states")]
        public async Task<IActionResult> GetUserVisitsStates(int user)
        {
            IActionResult response = null;
            this._logger.LogInformation(LoggingEvents.GET_USER_VISITS_STATES, "Getting user visits states for userId={user}", user);

            var tasks = new List<Task>();
            var statesTask = this.VisitsRepository.GetVisitsDistinctStateIds(user);
            tasks.Add(statesTask);
            var stateCacheTask = this.EnsureStateCacheIsPopulated();
            tasks.Add(stateCacheTask);

            //Do all these tasks at once and wait for them to complete.
            await Task.WhenAll(tasks.ToArray());

            var stateIds = statesTask.Result.ToArray();

            if (stateIds.Length > 0)
            {
                var stateAbbreviations = stateIds.Select(sid => _statesCache.Where(s => s.StateId == sid).FirstOrDefault()).ToArray();
                response = this.Ok(stateAbbreviations);
            }
            else
            {
                response = this.NotFound();
            }

            return response;
        }

        [HttpGet()]
        [Route("user/{user}/visit/{visit}")]
        public async Task<IActionResult> GetUserVisit(int user, string visit)
        {
            IActionResult response = null;
            this._logger.LogInformation(LoggingEvents.GET_USER_VISIT, "Getting user visit for userId={user} and visitId={visit}", user, visit);
            var userVisit = await this.VisitsRepository.GetVisit(visit);

            //Validate
            if (userVisit != null && userVisit.User == user)
            {
                var tasks = new List<Task>();
                var cityTask = this.GeographyRepository.GetCityAsync(userVisit.CityId);
                var stateTask = this.GeographyRepository.GetStateAsync(userVisit.StateId);
                tasks.Add(cityTask);
                tasks.Add(stateTask);
                await Task.WhenAll(tasks.ToArray());
                var city = cityTask.Result;
                var state = stateTask.Result;

                VisitRepresentation visitRepresentation = new VisitRepresentation()
                {
                    User = userVisit.User,
                    Created = userVisit.Created,
                    City = city.Name,
                    State = state.Abbreviation,
                    VisitId = userVisit.VisitId,
                    Links = new VisitRepresentationLinks()
                    {
                        Self = new Link()
                        {
                            Href = string.Format("/user/{0}/visit/{1}", userVisit.User, userVisit.VisitId)
                        }
                    }
                };
                response = this.Ok(visitRepresentation);
            }
            else
            {
                response = this.NotFound();
            }

            return response;
        }
        
        [HttpPost]
        [Route("user/{user}/visits")]
        public async Task<IActionResult> PostUserVisit(int user, [FromBody]PostVisitRepresentation visit)
        {
            IActionResult response = null;
            this._logger.LogInformation(LoggingEvents.POST_USER_VISIT, "Post user visit for userId={user}", user);
            var claims = this.ExtractClaimsFromAuthorizationHeaderBearerToken(this.Request.Headers);

            //NOTE: Right now this always evaluates to true, because the BearerTokenDecryptor is hard coded to
            //return this claim.
            if (ClaimsChecker.IsAllowed("POST", "*", claims))
            {
                var claimsUserId = this.ExtractClaimsUserId(claims);
                if (claimsUserId.HasValue && user == claimsUserId.Value)
                {
                    var tasks = new List<Task>();
                    var stateTask = this.GeographyRepository.GetStateByAbbreviationAsync(visit.State);
                    tasks.Add(stateTask);
                    var cityTask = this.GeographyRepository.GetCityAsync(visit.State, visit.City);
                    tasks.Add(cityTask);
                    var state = stateTask.Result;
                    var city = cityTask.Result;

                    //Do all these tasks at once and wait for all to complete.
                    await Task.WhenAll(tasks);

                    //Persist to the repository
                    var userVisit = new Visit()
                    {
                        Created = DateTime.UtcNow,
                        User = user,
                        CityId = city.CityId,
                        StateId = state.StateId,
                        VisitId = Guid.NewGuid().ToString()
                    };
                    await this.VisitsRepository.SaveVisit(userVisit);


                    //Convert to representation.
                    var visitRepresentation = new VisitRepresentation()
                    {
                        City = city.Name,
                        Created = userVisit.Created,
                        State = state.Abbreviation,
                        User = user,
                        VisitId = userVisit.VisitId,
                        Links = new VisitRepresentationLinks()
                        {
                            Self = new Link()
                            {
                                Href = string.Format("/user/{0}/visit/{1}", userVisit.User, userVisit.VisitId)
                            }
                        }
                    };

                    response = this.Ok(visitRepresentation);
                }
                else
                {
                    this._logger.LogWarning(LoggingEvents.POST_USER_VISIT, "Unauthorized attempt to post user visit for userId={user}", user);
                    response = this.Unauthorized();
                }
            }
            else
            {
                this._logger.LogWarning(LoggingEvents.POST_USER_VISIT, "Unauthorized attempt to post user visit for userId={user}", user);
                response = this.Unauthorized();
            }

            return response;
        }
        
        [HttpDelete("user/{user}/visit/{visit}")]
        public async Task<IActionResult> DeleteUserVisit(int user, string visit)
        {
            IActionResult response = null;
            this._logger.LogInformation(LoggingEvents.DELETE_USER_VISIT, "Delete user visit for userId={user} and visitId={visit}", user, visit);
            var claims = this.ExtractClaimsFromAuthorizationHeaderBearerToken(this.Request.Headers);

            //NOTE: Right now this always evaluates to true, because the BearerTokenDecryptor is hard coded to
            //return this claim.
            if (ClaimsChecker.IsAllowed("DELETE", "*", claims))
            {
                await this.VisitsRepository.DeleteVisit(user, visit);

                //200 OK is always returned unless there is an internal server error, because this is an
                //idempotent call. If there is an error, the framework will auto reply with 500 internal server error.
                response = this.Ok();
            }
            else
            {
                this._logger.LogWarning(LoggingEvents.DELETE_USER_VISIT, "Unauthorized attempt to delete user visit for userId={user} and visitId={visit}", user, visit);
                response = this.Unauthorized();
            }

            return response;
        }

        private async Task EnsureCityCacheIsPopulated()
        {
            //Retrieve cities for the first time, if they haven't already. These can be cached because
            //the dataset is relatively small and doesn't change often.
            if (_citiesCache == null)
            {
                this._logger.LogInformation(LoggingEvents.POPULATING_CITY_CACHE, "Populating city cache.");
                _citiesCache = (await this.GeographyRepository.GetCitiesAsync()).ToArray();
            }
        }

        private async Task EnsureStateCacheIsPopulated()
        {
            //Retrieve states for the first time, if they haven't already. These can be cached because
            //the dataset is small and doesn't change often.
            if (_statesCache == null)
            {
                this._logger.LogInformation(LoggingEvents.POPULATING_STATE_CACHE, "Populating state cache.");
                _statesCache = (await this.GeographyRepository.GetStatesAsync()).ToArray();
            }
        }
    }
}

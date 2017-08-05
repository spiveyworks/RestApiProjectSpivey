using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Visits;
using VisitsRepository;
using GeographyRepository;

namespace WebApi.Controllers
{
    public class UserVisitsController : Controller
    {
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

        public UserVisitsController(IVisitsRepository visitsRepository, IGeographyRepository geographyRepository)
        {
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
                        User = visit.User
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
            
            var tasks = new List<Task>();
            var statesTask = this.VisitsRepository.GetVisitsDistinctStateIds(user);
            tasks.Add(statesTask);
            var stateCacheTask = this.EnsureStateCacheIsPopulated();
            tasks.Add(stateCacheTask);
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
                    State = state.Abbreviation
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
            var tasks = new List<Task>();
            var stateTask = this.GeographyRepository.GetStateByAbbreviationAsync(visit.State);
            tasks.Add(stateTask);
            var cityTask = this.GeographyRepository.GetCityAsync(visit.State, visit.City);
            tasks.Add(cityTask);
            var state = stateTask.Result;
            var city = cityTask.Result;

            await Task.WhenAll(tasks);

            var userVisit = new Visit()
            {
                Created = DateTime.UtcNow,
                User = user,
                CityId = city.CityId,
                StateId = state.StateId,
                VisitId = Guid.NewGuid().ToString()
            };
            await this.VisitsRepository.SaveVisit(userVisit);
            return this.Ok();
        }
        
        [HttpDelete("user/{user}/visit/{visit}")]
        public async Task<IActionResult> DeleteUserVisit(int user, string visit)
        {
            return this.Ok();
        }

        private async Task EnsureCityCacheIsPopulated()
        {
            //Retrieve cities for the first time, if they haven't already. These can be cached because
            //the dataset is relatively small and doesn't change often.
            if (_citiesCache == null)
                _citiesCache = (await this.GeographyRepository.GetCitiesAsync()).ToArray();
        }

        private async Task EnsureStateCacheIsPopulated()
        {
            //Retrieve states for the first time, if they haven't already. These can be cached because
            //the dataset is small and doesn't change often.
            if (_statesCache == null)
                _statesCache = (await this.GeographyRepository.GetStatesAsync()).ToArray();
        }
    }
}

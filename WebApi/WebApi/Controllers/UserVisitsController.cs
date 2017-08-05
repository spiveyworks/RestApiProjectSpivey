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

            var visits = (await this.VisitsRepository.GetVisitsByUserId(user, skip, take)).ToArray();

            if (visits.Length > 0)
            {
                response = this.Ok(visits);
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
            var visits = new List<string>();
            response = this.Ok(visits);
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
            return this.Ok();
        }
        
        [HttpDelete("user/{user}/visit/{visit}")]
        public async Task<IActionResult> DeleteUserVisit(int user, string visit)
        {
            return this.Ok();
        }
    }
}

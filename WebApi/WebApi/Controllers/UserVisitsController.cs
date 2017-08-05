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
        public async Task<IActionResult> GetUserVisits(string user)
        {
            IActionResult response = null;
            var visits = new List<string>();
            response = this.Ok(visits);
            return response;
        }

        [HttpGet]
        [Route("user/{user}/visits/states")]
        public async Task<IActionResult> GetUserVisitsStates(string user)
        {
            IActionResult response = null;
            var visits = new List<string>();
            response = this.Ok(visits);
            return response;
        }

        [HttpGet()]
        [Route("user/{user}/visit/{visit}")]
        public async Task<IActionResult> GetUserVisit(string user, string visit)
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
        public async Task<IActionResult> PostUserVisit(string user, [FromBody]PostVisitRepresentation visit)
        {
            return this.Ok();
        }
        
        [HttpDelete("user/{user}/visit/{visit}")]
        public async Task<IActionResult> DeleteUserVisit(string user, string visit)
        {
            return this.Ok();
        }
    }
}

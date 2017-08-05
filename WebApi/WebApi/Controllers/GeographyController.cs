using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeographyRepository;
using WebApi.Geography;

namespace WebApi.Controllers
{
    public class GeographyController : BaseController
    {
        private IGeographyRepository _geographyRepository;

        public IGeographyRepository GeographyRepository
        {
            get { return this._geographyRepository; }
        }

        public GeographyController(IGeographyRepository geographyRepository)
        {
            this._geographyRepository = geographyRepository;
        }

        [HttpGet]
        [Route("state/{state}/cities")]
        public async Task<IActionResult> GetStateCities(string state)
        {
            IActionResult response = null;

            //Input validation
            if (state.Length == 2)
            {
                var stateUpper = state.ToUpper();

                if (await GeographyRepository.DoesStateExistAsync(stateUpper))
                {
                    var cities = (await GeographyRepository.GetCitiesAsync(state)).ToArray();
                    var stateCitiesRepresentation = new StateCitiesRepresentation()
                    {
                        State = state.ToUpper()
                    };

                    if (cities.Length > 0)
                        stateCitiesRepresentation.Cities = cities.Select(c => c.Name).ToArray();

                    response = this.Ok(stateCitiesRepresentation);
                }
                else
                {
                    response = this.NotFound();
                }
            }
            else
            {
                response = this.BadRequest();
            }
            
            return response;
        }

    }
}

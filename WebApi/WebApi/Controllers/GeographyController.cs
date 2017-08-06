using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeographyRepository;
using WebApi.Geography;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    public class GeographyController : BaseController
    {
        private readonly ILogger _logger;
        private IGeographyRepository _geographyRepository;

        public IGeographyRepository GeographyRepository
        {
            get { return this._geographyRepository; }
        }

        public GeographyController(ILogger<GeographyController> logger, IGeographyRepository geographyRepository)
        {
            this._logger = logger;
            this._geographyRepository = geographyRepository;
        }

        [HttpGet]
        [Route("state/{state}/cities")]
        public async Task<IActionResult> GetStateCities(string state)
        {
            IActionResult response = null;
            this._logger.LogInformation(LoggingEvents.GET_STATE_CITIES, "Get cities for state={state}", state);

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

                    //TODO: Should make the following configurable. Also, if is for unit test to complete.
                    if (this.Response != null && this.Response.Headers != null)
                        this.Response.Headers.Add("Cache-Control", "public, max-age=31536000");
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeographyRepository;

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
            if (await GeographyRepository.DoesStateExistAsync(state))
            {
                var cities = await GeographyRepository.GetCitiesAsync(state);
                response = this.Ok(cities);
            }
            else
            {
                response = this.NotFound();
            }
            
            return response;
        }
        
    }
}

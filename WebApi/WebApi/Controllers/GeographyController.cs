using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class GeographyController : Controller
    {
        [HttpGet]
        [Route("state/{state}/cities")]
        public async Task<IActionResult> GetStateCities(string state)
        {
            IActionResult response = null;
            var cities = new List<string>();
            response = this.Ok(cities);
            return response;
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Visits;

namespace WebApi.Controllers
{
    public class UserVisitsController : Controller
    {
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
            VisitRepresentation visitRepresentation = new VisitRepresentation();
            response = this.Ok(visitRepresentation);
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

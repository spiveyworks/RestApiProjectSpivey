using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Authorization;

namespace WebApi.Controllers
{
    public class BaseController : Controller
    {
        protected string[] ExtractClaimsFromAuthorizationHeaderBearerToken(IHeaderDictionary headers)
        {
            var claims = new String[0];

            try
            {
                var authorizationHeader = headers["Authorization"].FirstOrDefault();
                var decryptor = new BearerTokenDecryptor();
                claims = decryptor.ExtractClaims(authorizationHeader);
            }
            catch { }

            return claims;
        }

        protected Nullable<int> ExtractClaimsUserId(string[] claims)
        {
            Nullable<int> userId = null;

            try
            {
                var userIdString = claims.Where(c => c.StartsWith("user=")).Select(c => c.Split('=')[1]).FirstOrDefault();
                int userIdVal;

                if (int.TryParse(userIdString, out userIdVal))
                    userId = new Nullable<int>(userIdVal);
            }
            catch { }

            return userId;
        }
    }
}

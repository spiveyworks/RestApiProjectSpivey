using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Authorization
{
    public static class ClaimsChecker
    {
        public static bool IsAllowed(string method, string resource, string[] claims)
        {
            var allowed = false;
            
            if (claims != null)
                allowed = claims.Contains(method + " " + resource);

            return allowed;
        }
    }
}

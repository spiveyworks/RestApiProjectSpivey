using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Authorization
{
    public class BearerTokenDecryptor
    {
        public string[] ExtractClaims(string bearerToken)
        {
            var claims = new List<string>();

            //TODO: Replace this hard coded list of everything with actually decrypting the bearer token
            //and extracting the claims inside.
            claims.Add("GET *");
            claims.Add("POST *");
            claims.Add("DELETE *");

            //TODO: This is hard coded to always be user 1 right now.
            claims.Add("user=1");

            return claims.ToArray();
        }
    }
}

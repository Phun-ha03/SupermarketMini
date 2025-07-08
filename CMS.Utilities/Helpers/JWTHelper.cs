using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Utilities.Helpers
{
    public class JWTHelper
    {
        public static string GenerateToken(string sub, int pid, int uid, string secret_key)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret_key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var secToken = new JwtSecurityToken(
                signingCredentials: credentials,
                //issuer: "issuer",
                //audience: "audience",
                claims: new[]
                {
                    new Claim("sub", sub),
                    new Claim("pid", pid.ToString()),
                    new Claim("uid", uid.ToString()),
                },
                expires: DateTime.UtcNow.AddDays(1));
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(secToken);
        }

        public static bool ValidateToken(string token, string secret_key, ref string sub, ref int pid, ref int uid)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters(secret_key);

            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

            if (principal.Identity != null && principal.Identity.IsAuthenticated)
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token);
                var tokenS = jsonToken as JwtSecurityToken;

                sub = tokenS.Claims.First(claim => claim.Type == "sub").Value;
                var pid_str = tokenS.Claims.First(claim => claim.Type == "pid").Value;
                var uid_str = tokenS.Claims.First(claim => claim.Type == "uid").Value;

                if (!string.IsNullOrEmpty(pid_str))
                {
                    pid = Convert.ToInt32(pid_str);
                }

                if (!string.IsNullOrEmpty(uid_str))
                {
                    uid = Convert.ToInt32(uid_str);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private static TokenValidationParameters GetValidationParameters(string secret_key)
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidIssuer = "issuer",
                ValidAudience = "audience",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret_key))
            };
        }

        public static void GetClaims(string token, ref string sub, ref int pid, ref int uid)
        {
            sub = string.Empty;
            pid = 0;
            uid = 0;

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;

            sub = tokenS.Claims.First(claim => claim.Type == "sub").Value;
            var pid_str = tokenS.Claims.First(claim => claim.Type == "pid").Value;
            var uid_str = tokenS.Claims.First(claim => claim.Type == "uid").Value;

            if (!string.IsNullOrEmpty(pid_str))
            {
                 int.TryParse(pid_str, out pid);
            }
            if (!string.IsNullOrEmpty(uid_str))
            {
                int.TryParse(uid_str, out uid);
            }
        }
    }
}

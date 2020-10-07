using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OMSWebMini.Services.Authorization;

namespace OMSWebMini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        ISigningSecurityKey signingSecurityKey;

        public AuthorizationController(ISigningSecurityKey signingSecurityKey)
        {
            this.signingSecurityKey = signingSecurityKey;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<string> Authorize()
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,"Bekzod.Allaev")
            };

            var token = new JwtSecurityToken(
                issuer: "OMSWebMini_IIS",
                audience: "OMSWebMini_IISAudience",
                expires: DateTime.Now.AddMinutes(30),
                claims: claims,
                signingCredentials: new SigningCredentials(
                    signingSecurityKey.GetKey(),
                    signingSecurityKey.SigningAlgorithm));

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}

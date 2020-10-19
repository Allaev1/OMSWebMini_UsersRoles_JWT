using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OMSWebMini.Authentication.Data;
using OMSWebMini.Authentication.Model;
using OMSWebMini.Services.Authorization;

namespace OMSWebMini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        ISigningSecurityKey signingSecurityKey;
        AuthenticationContext context;

        public AuthorizationController(ISigningSecurityKey signingSecurityKey,AuthenticationContext context)
        {
            this.signingSecurityKey = signingSecurityKey;
            this.context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Authorize(AuthenticationModel model)
        {
            ApplicationUser applicationUser = await context.Users.SingleOrDefaultAsync(a => a.Login == model.Login);

            if (applicationUser == null)
                return Unauthorized();
            else if (applicationUser.PasswordHash != model.Password)
                return Unauthorized();

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

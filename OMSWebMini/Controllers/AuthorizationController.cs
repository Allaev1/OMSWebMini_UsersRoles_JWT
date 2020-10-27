using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        UserManager<ApplicationUser> userManager;

        public AuthorizationController(ISigningSecurityKey signingSecurityKey, UserManager<ApplicationUser> userManager)
        {
            this.signingSecurityKey = signingSecurityKey;
            this.userManager = userManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Authorize(AuthenticationModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);

            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,model.UserName)
                };

                foreach (var userRole in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                }

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

            return Unauthorized();
        }
    }
}

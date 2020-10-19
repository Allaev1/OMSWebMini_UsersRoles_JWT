using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using OMSWebMini.Authentication.Data;
using OMSWebMini.Authentication.Model;

namespace OMSWebMini.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        AuthenticationContext context;

        public RegistrationController(AuthenticationContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Register(AuthenticationModel registrationModel)
        {
            bool userExist = await context.Users.AnyAsync(a => a.Login == registrationModel.Login);

            if (userExist) return BadRequest();

            bool passwordExist = await context.Users.AnyAsync(a => a.PasswordHash == registrationModel.Password);

            if (passwordExist) return BadRequest();

            ApplicationUser applicationUser = new ApplicationUser()
            {
                Login = registrationModel.Login,
                PasswordHash = registrationModel.Password
            };

            context.Users.Add(applicationUser);

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}

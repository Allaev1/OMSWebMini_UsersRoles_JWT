using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        UserManager<ApplicationUser> userManager;
        RoleManager<IdentityRole> roleManager;

        public RegistrationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> RegisterUser(RegistrationModel registrationModel)
        {
            var user = await userManager.FindByNameAsync(registrationModel.UserName);

            if (user != null) return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Such username already exists" });

            ApplicationUser applicationUser = new ApplicationUser()
            {
                UserName = registrationModel.UserName,
                Email = registrationModel.Email
            };

            var result = await userManager.CreateAsync(applicationUser, registrationModel.Password);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            await AddToRoleAsync(applicationUser, UserRoles.User);

            return Ok(new Response { Status = "Success", Message = "User created" });
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<Response>> RegisterAdmin(RegistrationModel registrationAdminModel)
        {
            var user = await userManager.FindByNameAsync(registrationAdminModel.UserName);

            if (user == null) return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Such username already exists" });

            ApplicationUser applicationUser = new ApplicationUser()
            {
                UserName = registrationAdminModel.UserName,
                Email = registrationAdminModel.Email
            };

            var result = await userManager.CreateAsync(applicationUser, registrationAdminModel.Password);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            await AddToRoleAsync(applicationUser, UserRoles.Admin);

            return Ok(new Response { Status = "Success", Message = "Admin created" });

        }

        private async Task AddToRoleAsync(ApplicationUser user, string role)
        {
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            await userManager.AddToRoleAsync(user, role);
        }
    }
}

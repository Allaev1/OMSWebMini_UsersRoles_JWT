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
        public async Task<ActionResult> RegisterUser(RegistrationModel registrationModel)
        {
            var user = await userManager.FindByNameAsync(registrationModel.UserName);

            if (user != null) return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Such username already exists" });

            if (!await roleManager.RoleExistsAsync(registrationModel.Role)) return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "No such role" });

            ApplicationUser applicationUser = new ApplicationUser()
            {
                UserName = registrationModel.UserName,
                Email = registrationModel.Email
            };

            var result = await userManager.CreateAsync(applicationUser, registrationModel.Password);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            await AddToRoleAsync(applicationUser, registrationModel.Role);

            return Ok(new Response { Status = "Success", Message = "User created" });
        }

        //[HttpPost]
        //[Route("[action]")]
        //public async Task<ActionResult<Response>> RegisterAdmin(RegistrationModel registrationAdminModel)
        //{
        //    var user = await userManager.FindByNameAsync(registrationAdminModel.UserName);

        //    if (user == null) return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Such username already exists" });

        //    ApplicationUser applicationUser = new ApplicationUser()
        //    {
        //        UserName = registrationAdminModel.UserName,
        //        Email = registrationAdminModel.Email
        //    };

        //    var result = await userManager.CreateAsync(applicationUser, registrationAdminModel.Password);

        //    if (!result.Succeeded)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

        //    await AddToRoleAsync(applicationUser, UserRoles.Admin);

        //    return Ok(new Response { Status = "Success", Message = "Admin created" });

        //}

        private async Task AddToRoleAsync(ApplicationUser user, string role)
        {
            if (!await roleManager.RoleExistsAsync(UserRoles.Founder))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Founder));
            if (!await roleManager.RoleExistsAsync(UserRoles.CustomerManager))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.CustomerManager));
            if (!await roleManager.RoleExistsAsync(UserRoles.HRManager))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.HRManager));
            if (!await roleManager.RoleExistsAsync(UserRoles.StoreManager))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.StoreManager));
            if (!await roleManager.RoleExistsAsync(UserRoles.SupplierManager))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.SupplierManager));
            if (!await roleManager.RoleExistsAsync(UserRoles.ShipperManager))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.ShipperManager));
            if (!await roleManager.RoleExistsAsync(UserRoles.OrderManager))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.OrderManager));
            if (!await roleManager.RoleExistsAsync(UserRoles.StatisticManager))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.StatisticManager));
            if (!await roleManager.RoleExistsAsync(UserRoles.Customer))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Customer));

            await userManager.AddToRoleAsync(user, role);
        }
    }
}

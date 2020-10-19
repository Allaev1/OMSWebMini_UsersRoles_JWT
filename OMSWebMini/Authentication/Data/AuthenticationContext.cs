using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OMSWebMini.Authentication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OMSWebMini.Authentication.Data
{
    //IdentityUserContext<T> class used when there is no need in roles in identity. Read this - https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-3.1#model-generic-types

    //In this project you have two database providers so you will need in this article which explains how to do migration in project with multiple database providers
    //https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/providers?tabs=dotnet-core-cli
    public class AuthenticationContext : IdentityUserContext<ApplicationUser>
    {
        public AuthenticationContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) { }
    }
}

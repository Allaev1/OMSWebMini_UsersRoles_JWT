using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OMSWebMini.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OMSWebMini.Services.Authorization;
using Microsoft.EntityFrameworkCore;
using OMSWebMini.Authentication.Data;
using OMSWebMini.Authentication.Model;
using Microsoft.AspNetCore.Identity;

namespace OMSWebMini
{
    public class Startup
    {
        const string SECURITY_KEY = "0d5b3235a8b403c3dab9c3f4f65c07fcalskd234n1k41230";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var signingSecurityKey = new SigningSecurityKey(SECURITY_KEY, SecurityAlgorithms.HmacSha256);
            services.AddSingleton<ISigningSecurityKey>(signingSecurityKey);

            services.AddControllersWithViews().
                AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "Version #1",
                    Title = "OMSWebMini",
                });
            });

            services.AddDbContext<NorthwindContext>();

            //Add database for identity
            services.AddDbContext<IdentityContext>(options => options.UseSqlite("Data Source = Identity.db"));

            //Add identity
            services.AddIdentity<ApplicationUser, IdentityRole>().
                AddEntityFrameworkStores<IdentityContext>().
                AddDefaultTokenProviders(); //This method generate token for email confirmation, reset password, change telephone number and two-factor authentication

            services.AddControllers();

            const string jwtSchemeName = "JwtBearer";
            services.AddAuthentication(options => { options.DefaultAuthenticateScheme = jwtSchemeName; options.DefaultChallengeScheme = jwtSchemeName; })
                .AddJwtBearer(jwtSchemeName, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "OMSWebMini_IIS",

                        ValidateAudience = true,
                        ValidAudience= "OMSWebMini_IISAudience",

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingSecurityKey.GetKey(),

                        ValidateLifetime = true
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

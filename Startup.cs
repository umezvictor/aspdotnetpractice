using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileManagerApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileManagerApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // configure dependency injection for DbContext of type UserContext
            services.AddDbContext<UserContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Devconnection")));

            // configure identity -- step 2 of usiing identity
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;// set the password length you want. it is 6 by default
                options.Password.RequiredUniqueChars = 3;
            })
                .AddEntityFrameworkStores<UserContext>();

            //override default form validation rules
            //configure password options
            // AdIdentity method can achieve the same functionality
           /* services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;// set the password length you want. it is 6 by default
                options.Password.RequiredUniqueChars = 3;
            });
            */

            // configure authorization globally
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();// step 3 to use identity -- next, create a migration table

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

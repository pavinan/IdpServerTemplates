using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdpServer.Application;
using IdpServer.Infrastructure;
using IdpServer.Models;
using IdpServer.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdpServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            // configures IIS in-proc settings
            services.Configure<IISServerOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            var migrationAssembly = typeof(ApplicationDbContext).Assembly.GetName().Name;
            var connString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connString, sql =>
                {
                    sql.MigrationsAssembly(migrationAssembly);
                });
            });


            services.AddDataProtection()
                .PersistKeysToDbContext<ApplicationDbContext>();

            var identityBuilder = services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>()
                .AddRoleManager<ApplicationRoleManager>()
                .AddUserManager<ApplicationUserManager>()
                .AddSignInManager<ApplicationSignInManager>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var identityServerBuilder = services.AddIdentityServer()
                .AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = o =>
                    {
                        o.UseSqlServer(connString, sql => sql.MigrationsAssembly(migrationAssembly));
                    };
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = o =>
                    {
                        o.UseSqlServer(connString, sql => sql.MigrationsAssembly(migrationAssembly));
                    };
                });

            identityServerBuilder.AddDeveloperSigningCredential(
                signingAlgorithm: IdentityServer4.IdentityServerConstants.RsaSigningAlgorithm.PS256
                );

            services.ConfigureNonBreakingSameSiteCookies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCookiePolicy();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}

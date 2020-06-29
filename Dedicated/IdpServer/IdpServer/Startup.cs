using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IdpServer.Application;
using IdpServer.ConfigModels;
using IdpServer.Infrastructure;
using IdpServer.Models;
using IdpServer.Persistence;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

            var identityBuilder = services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
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

                    options.EnableTokenCleanup = true;
                });

            identityServerBuilder.AddDeveloperSigningCredential(
                signingAlgorithm: IdentityServer4.IdentityServerConstants.RsaSigningAlgorithm.PS256
                );

            services.ConfigureNonBreakingSameSiteCookies();

            services.AddOptions();
            services.Configure<AppConfig>(Configuration);


            services.AddCors(o =>
            {
                o.AddPolicy("app", b =>
                {
                    b.AllowAnyHeader();
                    b.AllowAnyMethod();
                    b.SetPreflightMaxAge(TimeSpan.FromSeconds(10));
                    b.SetIsOriginAllowed(x => true);
                    b.AllowCredentials();
                });
            });

            services.AddAuthentication()
                .AddJwtBearer(x =>
                {
                    x.Authority = Configuration["AppUrl"];
                    x.Audience = "identity";
                    
                });

            services.AddAuthorization(c =>
            {
                c.AddPolicy("api", p =>
                {
                    p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    p.RequireAuthenticatedUser();
                });
            });

            services.AddOData();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var forwardingOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All,
                ForwardLimit = null
            };

            forwardingOptions.KnownNetworks.Clear();
            forwardingOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(forwardingOptions);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCookiePolicy();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();
            app.UseCors("app");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();

                endpoints.Select().Filter().OrderBy().Count().MaxTop(10);
                endpoints.EnableDependencyInjection();
                endpoints.MapODataRoute("v1.0", "v1.0", ODataHelpers.GetEdmModel());
            });
        }


    }
}

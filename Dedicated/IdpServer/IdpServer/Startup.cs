using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdpServer.Application;
using IdpServer.Application.Services;
using IdpServer.BuildingBlocks.AutoMapper;
using IdpServer.ConfigModels;
using IdpServer.Infrastructure;
using IdpServer.Infrastructure.Services;
using IdpServer.Models;
using IdpServer.Persistence;
using MediatR;
using MediatR.Pipeline;
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
using Newtonsoft.Json.Serialization;

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
            services.AddControllersWithViews().AddNewtonsoftJson(o=> 
            {
                o.AllowInputFormatterExceptionMessages = true;
                o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

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
                    b.AllowCredentials();

                    var origins = Configuration.GetSection("Cors").Get<string[]>() ?? new string[] { };

                    b.WithOrigins(origins);
                });
            });

            services.AddAuthentication()
                .AddJwtBearer(x =>
                {
                    x.Authority = Configuration["AppUrl"];

                    x.TokenValidationParameters.ValidateAudience = false;
                });

            services.AddAuthorization(c =>
            {
                c.AddPolicy("api", p =>
                {
                    p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    p.RequireAuthenticatedUser();
                });
            });

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile(typeof(ApplicationUserManager).Assembly));
            });

            services.AddSingleton(mapperConfiguration.CreateMapper());

            services.AddOData();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IIdentityService, IdentityService>();

            var assembly = typeof(ApplicationUserManager).Assembly;
            services.AddMediatR(assembly);
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

                endpoints.Select().Filter().OrderBy().Count().MaxTop(1000);
                //endpoints.EnableDependencyInjection();
                endpoints.MapODataRoute("api", "api", cb => ODataHelper.GetEdmModel(app.ApplicationServices, cb));
            });
        }


    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Angular.Server.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProxyKit;

namespace Angular
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddProxy();
            services.AddControllers();
            services.AddOptions();
            services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (!env.IsDevelopment())
            {
                app.Use(async (context, next) =>
                {
                    await next();

                    // If there's no available file and the request doesn't contain an extension, we're probably trying to access a page.
                    // Rewrite request to use app root
                    if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value) && !context.Request.Path.Value.StartsWith("/api"))
                    {
                        context.Request.Path = "/index.html";
                        context.Response.StatusCode = 200; // Make sure we update the status code, otherwise it returns 404
                        await next();
                    }
                });
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });

            if (env.IsDevelopment())
            {
                app.MapWhen(x => x.Response.StatusCode == 404, appProxy =>
                {
                    app.UseWebSockets();
                    app.UseWebSocketProxy(
                      context => new Uri("ws://localhost:4200"),
                      options => options.AddXForwardedHeaders());

                    app.RunProxy(context => context
                    .ForwardTo("http://localhost:4200")
                    .AddXForwardedHeaders()
                    .Send());

                });
            }
        }
    }
}

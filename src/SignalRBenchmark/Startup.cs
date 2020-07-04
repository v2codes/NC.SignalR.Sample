using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRBenchmark.Hubs;

namespace SignalRBenchmark
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly bool useAzureSignalR = true;

        public Startup(IConfiguration config)
        {
            _config = config;
            useAzureSignalR = config.GetSection("Azure:SignalR:ConnectionString").Exists();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var signalrBuilder = services.AddSignalR(config =>
            {
                config.EnableDetailedErrors = true;
            });

            if (useAzureSignalR)
            {
                signalrBuilder.AddAzureSignalR();
            }

            // TODO: Json vs NewtonsoftJson option
            signalrBuilder.AddMessagePackProtocol();

            var redisConnectionString = _config["SignalRRedis"];
            if (!string.IsNullOrEmpty(redisConnectionString))
            {
                //signalrBuilder.AddStackExchangeRedis(redisConnectionString);
            }

            services.AddSingleton<ConnectionCounter>();

            //services.AddHostedService<HostedCounterService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //¾²Ì¬ÎÄ¼þ
            app.UseStaticFiles();
            ////Ä¿Â¼ä¯ÀÀ
            //app.UseDirectoryBrowser();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
                endpoints.MapControllers();

                if (useAzureSignalR)
                {
                    app.UseAzureSignalR(routes =>
                    {
                        routes.MapHub<EchoHub>("/echo");
                    });
                }
                else
                {
                    endpoints.MapHub<EchoHub>("/echo", o =>
                    {
                        // Remove backpressure for benchmarking
                        o.TransportMaxBufferSize = 0;
                        o.ApplicationMaxBufferSize = 0;
                    });
                }
            });
        }
    }
}

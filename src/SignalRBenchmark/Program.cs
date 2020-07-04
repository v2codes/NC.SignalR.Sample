using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SignalRBenchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Process ID:{0}", Process.GetCurrentProcess().Id);
            CreateHostBuilder(args).Build().Run();

            //Console.WriteLine($"Process ID: {Process.GetCurrentProcess().Id}");

            //var config = new ConfigurationBuilder()
            //    .AddEnvironmentVariables(prefix: "ASPNETCORE_")
            //    .AddCommandLine(args)
            //    .Build();

            //var host = new WebHostBuilder()
            //    .UseConfiguration(config)
            //    .ConfigureLogging(loggerFactory =>
            //    {
            //        loggerFactory.AddConsole().AddFilter("Microsoft", LogLevel.Warning);
            //    })
            //    .UseKestrel()
            //    .UseStartup<Startup>();

            //host.Build().Run();
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureLogging(loggerFactory =>
        //        {
        //            loggerFactory.AddConsole().AddFilter("Microsoft", LogLevel.Warning);
        //        })
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            var config = new ConfigurationBuilder()
        //                            .AddEnvironmentVariables(prefix: "ASPNETCORE_")
        //                            .AddCommandLine(args)
        //                            .Build();

        //            webBuilder.UseConfiguration(config)
        //                      .UseKestrel()
        //                      .UseStartup<Startup>();
        //        });


        public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
    }
}

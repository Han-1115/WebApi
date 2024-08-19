using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BCS.Core.Configuration;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting.WindowsServices;
using System.Reflection;

namespace BCS.WebApi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var options = new WebApplicationOptions
            {
                Args = args,
                ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : Directory.GetCurrentDirectory()
            };
            var host = CreateHostBuilder(options).Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(WebApplicationOptions applicationOptions) =>
               Host.CreateDefaultBuilder(applicationOptions.Args)
                   .UseWindowsService()
                   .ConfigureWebHostDefaults(webBuilder =>
                   {
                       webBuilder.UseContentRoot(applicationOptions.ContentRootPath);
                       webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                       {
                           var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                           Console.WriteLine($"ASPNETCORE_ENVIRONMENT:{env}");
                           config.AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true);
                       });
                       webBuilder.UseKestrel((hostingContext, options) =>
                       {
                           var kestrelSection = hostingContext.Configuration.GetSection("Kestrel");
                           options.Configure(kestrelSection);
                       });
                       webBuilder.UseIIS();
                       webBuilder.UseStartup<Startup>();
                   }).UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Autofac.Extensions.DependencyInjection;
using Serilog;
using Bussy.Server.Extensions.Host;

namespace Bussy.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.AddLoggingConfiguration();

            try
            {
                Log.Information("Starting Bussy Server...");
                await host.RunAsync();
            }
            catch (Exception e)
            {
                Log.Error(e, "The Application Failed to Start Correctly!");
                throw;
            }
            finally
            {
                Log.Information("Shutting Down!");
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup(typeof(Startup).GetTypeInfo().Assembly.FullName!)
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseKestrel();
                });
    }
}
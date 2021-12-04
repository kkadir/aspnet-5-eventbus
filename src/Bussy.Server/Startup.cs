using System;
using Bussy.Server.Databases;
using Bussy.Server.Extensions.Application;
using Bussy.Server.Extensions.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;

namespace Bussy.Server
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Log.Logger);
            services.AddCorsService("BussyCorsPolicy", _environment);
            services.AddDatabaseService(_configuration, _environment);
            services.AddControllers()
                .AddNewtonsoftJson(options => 
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddApiVersioningService();
            services.AddWebApiServices();
            services.AddHealthChecks();
            services.AddSwaggerService(_configuration);
        }
        
        public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();

                using var context = applicationBuilder.ApplicationServices.GetService<BussyDbContext>() 
                                    ?? throw new SystemException("Database Service Cannot Be Found!");
                context.Database.EnsureCreated();
            }
            else
            {
                applicationBuilder.UseExceptionHandler("/Error");
                applicationBuilder.UseHsts();
            }

            applicationBuilder.UseHttpsRedirection();
            applicationBuilder.UseCors();
            applicationBuilder.UseSerilogRequestLogging();
            applicationBuilder.UseRouting();

            applicationBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/api/health");
                endpoints.MapControllers();
            });

            applicationBuilder.UseSwaggerApplication(_configuration);
        }
    }
}
using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

namespace Bussy.Server.Extensions.Services
{
    public static class SwaggerServiceExtension
    {
        public static void AddSwaggerService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Bussy API",
                        Description = "Bussy API uses a REST based design, leverages the JSON data format, and relies upon HTTPS for transport. " +
                                      "We respond with meaningful HTTP response codes and if an error occurs, we include error details in the response body." +
                                      "We also demonstrate how to include domain events sent over an event bus on save actions.",
                        Contact = new OpenApiContact
                        {
                            Name = "Bussy",
                            Email = "kadir.kilicoglu@outlook.com",
                            Url = new Uri("http://www.kadir.co"),
                        },
                    });
            });
        }
        
    }
}
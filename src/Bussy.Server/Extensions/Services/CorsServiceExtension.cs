using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bussy.Server.Extensions.Services
{
    public static class CorsServiceExtension
    {
        public static void AddCorsService(this IServiceCollection services, string policyName,
            IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(policyName, builder =>
                        builder.SetIsOriginAllowed(_ => true)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .WithExposedHeaders("X-Pagination"));
                });
            }
        }
    }
}
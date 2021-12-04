using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Bussy.Server.Extensions.Application
{
    public static class SwaggerApplicationExtension
    {
        public static void UseSwaggerApplication(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "");
                config.DocExpansion(DocExpansion.None);
            });
        }
    }
}
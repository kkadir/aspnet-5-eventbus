using Bussy.Server.Databases;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bussy.Server.Extensions.Services
{
    public static class DatabaseServiceExtension
    {
        public static void AddDatabaseService(this IServiceCollection services, IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                services.AddDbContext<BussyDbContext>(options =>
                    options.UseInMemoryDatabase("InMemBussyDb"));
            }
        }
    }
}
using System.Reflection;
using Bussy.Server.Services;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;
using FluentValidation.AspNetCore;
using MediatR;
using Bussy.Server.Middleware;

namespace Bussy.Server.Extensions.Services
{
    public static class WebApiServiceExtension
    {
        public static void AddWebApiServices(this IServiceCollection services)
        {
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddHttpContextAccessor();
            services.AddMediatR(typeof(Startup));
            services.AddScoped<SieveProcessor>();
            services.AddMvc(options => options.Filters.Add<ErrorHandlerFilterAttribute>())
                .AddFluentValidation(configuration => { configuration.AutomaticValidationEnabled = false; });
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
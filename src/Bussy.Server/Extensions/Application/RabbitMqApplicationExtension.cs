using Bussy.Server.IntegrationEvents.Events;
using Bussy.Server.Messaging.EventBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bussy.Server.Extensions.Application
{
    public static class RabbitMqApplicationExtension
    {
        public static void UseRabbitMqApplication(this IApplicationBuilder app)
        {
            // Assign handlers here for incoming events.
            
            // var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            // eventBus.Subscribe<OnThisEvent, CallThatEventHandler>();
        }
    }
}
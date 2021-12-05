using System.Threading.Tasks;
using Bussy.Server.Messaging.EventBus.Events;

namespace Bussy.Server.Messaging.EventBus
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IIntegrationEventHandler
    {
    }
}
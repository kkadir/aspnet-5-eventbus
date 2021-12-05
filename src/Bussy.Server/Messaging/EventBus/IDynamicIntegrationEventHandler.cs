using System.Threading.Tasks;

namespace Bussy.Server.Messaging.EventBus
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
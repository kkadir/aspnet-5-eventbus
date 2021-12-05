using System;
using RabbitMQ.Client;

namespace Bussy.Server.Messaging.EventBusRabbitMq
{
    public interface IRabbitMqPersistentConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
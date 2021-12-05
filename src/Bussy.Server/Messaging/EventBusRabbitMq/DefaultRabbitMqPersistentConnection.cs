using System;
using System.IO;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Bussy.Server.Messaging.EventBusRabbitMq
{
    public class DefaultRabbitMqPersistentConnection : IRabbitMqPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<DefaultRabbitMqPersistentConnection> _logger;
        private readonly int _retryCount;
        private IConnection _connection;
        private bool _disposed;

        private object _sync = new object();

        public DefaultRabbitMqPersistentConnection(
            IConnectionFactory connectionFactory,
            ILogger<DefaultRabbitMqPersistentConnection> logger, int retryCount = 3)
        {
        }
    }
}
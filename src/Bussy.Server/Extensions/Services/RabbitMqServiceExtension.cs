using System;
using System.Net.Security;
using System.Security.Authentication;
using Autofac;
using Bussy.Server.Messaging.EventBus;
using Bussy.Server.Messaging.EventBusRabbitMq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Bussy.Server.Extensions.Services
{
    public static class RabbitMqServiceExtension
    {
        public static void AddRabbitMqPersistenConnectionService(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment)
        {
            services.AddSingleton<IRabbitMqPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMqPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = configuration["EventBusConnection"],
                    Port = Convert.ToInt32(configuration["EventBusConnectionPort"]),
                    VirtualHost = configuration["EventBusConnectionVirtualHost"],
                    DispatchConsumersAsync = true
                };
                
                factory.Ssl.Version = SslProtocols.Tls12 | SslProtocols.Ssl3 | SslProtocols.Tls11 | SslProtocols.Ssl2;
                factory.Ssl.Enabled = true;
                factory.Ssl.AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateChainErrors
                                                     | SslPolicyErrors.RemoteCertificateNameMismatch
                                                     | SslPolicyErrors.RemoteCertificateNotAvailable;
                // factory.Ssl.Enabled = true;
                 factory.Ssl.CertificateValidationCallback = (sender, certificate, chain, errors) => true;

                

                if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
                {
                    factory.UserName = configuration["EventBusUserName"];
                }

                if (!string.IsNullOrEmpty(configuration["EventBusPassword"]))
                {
                    factory.Password = configuration["EventBusPassword"];
                }

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(configuration["EventBusRetryCount"]);
                }

                return new DefaultRabbitMqPersistentConnection(factory, logger, retryCount);
            });
        }

        public static void AddRabbitMqEventBusService(this IServiceCollection services, IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            services.AddSingleton<IEventBus, EventBusRabbitMq>(sp =>
            {
                var subscriptionClientName = configuration["SubscriptionClientName"];
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMqPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMq>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(configuration["EventBusRetryCount"]);
                }

                return new EventBusRabbitMq(rabbitMQPersistentConnection, logger, iLifetimeScope,
                    eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            });
            
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            
            // Add listened event handlers with transient life-scopes
            // services.AddTransient<CallThatHandler>();
            
        }
    }
}
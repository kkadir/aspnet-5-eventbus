using System;
using Bussy.Server.Messaging.EventBus.Events;

namespace Bussy.Server.IntegrationEvents.Events
{
    public record OrderCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid Id { get; set; }
        
        public string Account { get; set; }
        public string Symbol { get; set; }
        public float Price { get; set; }
        public int Size { get; set; }

        public OrderCreatedIntegrationEvent(Guid id, string account, string symbol, float price, int size)
        {
            Id = id;
            Account = account;
            Symbol = symbol;
            Price = price;
            Size = size;
        }
    }
}
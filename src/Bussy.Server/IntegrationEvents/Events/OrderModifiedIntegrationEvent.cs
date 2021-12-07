using System;
using Bussy.Server.Messaging.EventBus.Events;

namespace Bussy.Server.IntegrationEvents.Events
{
    public record OrderModifiedIntegrationEvent : IntegrationEvent
    {
        public Guid Id { get; set; }
        
        public string OldAccount { get; set; }
        public string NewAccount { get; set; }
        public string OldSymbol { get; set; }
        public string NewSymbol { get; set; }
        public float OldPrice { get; set; }
        public float NewPrice { get; set; }
        public int OldSize { get; set; }
        public int NewSize { get; set; }
        
        public bool IsDeleted { get; set; }

        public OrderModifiedIntegrationEvent(Guid id, 
            string oldAccount, string oldSymbol, float oldPrice, int oldSize,
            string newAccount, string newSymbol, float newPrice, int newSize,
            bool isDeleted)
        {
            Id = id;
            OldAccount = oldAccount;
            OldSymbol = oldSymbol;
            OldPrice = oldPrice;
            OldSize = oldSize;
            NewAccount = newAccount;
            NewSymbol = newSymbol;
            NewPrice = newPrice;
            NewSize = newSize;
            IsDeleted = isDeleted;
        }
    }
}
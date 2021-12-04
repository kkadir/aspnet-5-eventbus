using Sieve.Attributes;

namespace Bussy.Server.Domain.Orders
{
    public class Order : AuditableEntity
    {
        [Sieve(CanFilter = true, CanSort = true)]
        public string Account { get; set; }
        
        [Sieve(CanFilter = true, CanSort = true)]
        public string Symbol { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public float Price { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int Size { get; set; }
    }
}
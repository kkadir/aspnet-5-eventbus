using System;

namespace Bussy.Server.Models.Order
{
    public class OrderModel
    {
        public Guid Id { get; set; }
        
        public string Account { get; set; }
        public string Symbol { get; set; }
        public float Price { get; set; }
        public int Size { get; set; }
        
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        
        public bool IsDeleted { get; set; }
    }
}
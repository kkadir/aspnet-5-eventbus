using System;
using System.ComponentModel.DataAnnotations;

namespace Bussy.Server.Domain
{
    public abstract class BaseEntity :
        ISoftDeletable
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsDeleted { get; set; }
    }
}
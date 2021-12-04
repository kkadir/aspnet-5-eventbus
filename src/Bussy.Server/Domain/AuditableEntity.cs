using System;
using Bussy.Server.Domain.Audit;

namespace Bussy.Server.Domain
{
    public abstract class AuditableEntity :
        BaseEntity,
        ICreationAudited,
        IModificationAudited,
        IDeletionAudited
    {
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
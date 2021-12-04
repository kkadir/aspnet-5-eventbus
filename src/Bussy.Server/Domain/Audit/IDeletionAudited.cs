using System;

namespace Bussy.Server.Domain.Audit
{
    public interface IDeletionAudited
    {
        string? DeletedBy { get; set; }
        
        DateTime? DeletedOn { get; set; }
    }
}
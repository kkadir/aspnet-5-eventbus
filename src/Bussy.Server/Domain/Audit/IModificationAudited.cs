using System;

namespace Bussy.Server.Domain.Audit
{
    public interface IModificationAudited
    {
        string? ModifiedBy { get; set; }
        
        DateTime? ModifiedOn { get; set; }
    }
}
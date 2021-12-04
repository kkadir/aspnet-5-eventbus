using System;

namespace Bussy.Server.Domain.Audit
{
    public interface ICreationAudited
    {
        string? CreatedBy { get; set; }
        
        DateTime CreatedOn { get; set; }
    }
}
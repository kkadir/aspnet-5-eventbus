using System;

namespace Bussy.Server.Domain
{
    public interface IDeletionAudited
    {
        string DeletedBy { get; set; }
        
        DateTime? DeletedOn { get; set; }
    }
}
using System;

namespace Bussy.Server.Domain
{
    public interface IModificationAudited
    {
        string ModifiedBy { get; set; }
        
        DateTime? ModifiedOn { get; set; }
    }
}
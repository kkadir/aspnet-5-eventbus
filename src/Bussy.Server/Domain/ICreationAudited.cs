using System;

namespace Bussy.Server.Domain
{
    public interface ICreationAudited
    {
        string CreatedBy { get; set; }
        
        DateTime CreatedOn { get; set; }
    }
}
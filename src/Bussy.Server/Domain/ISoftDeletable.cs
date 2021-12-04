namespace Bussy.Server.Domain
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }
}
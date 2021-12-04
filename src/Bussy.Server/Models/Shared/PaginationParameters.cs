namespace Bussy.Server.Models.Shared
{
    public abstract class PaginationParameters
    {
        protected virtual int MaxPageSize { get; } = 20;
        
        protected virtual int DefaultPageSize { get; set; } = 10;

        public virtual int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => DefaultPageSize;
            set => DefaultPageSize = value > MaxPageSize ? MaxPageSize : value;
        }
        
    }
}
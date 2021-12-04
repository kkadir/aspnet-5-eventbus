using Bussy.Server.Models.Shared;

namespace Bussy.Server.Models.Order
{
    public class OrderParametersModel : PaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
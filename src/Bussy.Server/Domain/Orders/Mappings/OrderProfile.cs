using AutoMapper;
using Bussy.Server.Models.Order;

namespace Bussy.Server.Domain.Orders.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderModel>()
                .ReverseMap();
            CreateMap<OrderModelForCreation, Order>();
            CreateMap<OrderModelForUpdate, Order>()
                .ReverseMap();
        }
    }
}
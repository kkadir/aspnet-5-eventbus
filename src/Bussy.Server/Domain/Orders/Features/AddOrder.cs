using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bussy.Server.Databases;
using Bussy.Server.Models.Order;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bussy.Server.Domain.Orders.Features
{
    public static class AddOrder
    {
        public class AddOrderCommand : IRequest<OrderModel>
        {
            public OrderModelForCreation OrderToAdd { get; set; }

            public AddOrderCommand(OrderModelForCreation orderToAdd)
            {
                OrderToAdd = orderToAdd;
            }
        }

        public class Handler : IRequestHandler<AddOrderCommand, OrderModel>
        {
            private readonly BussyDbContext _db;
            private readonly IMapper _mapper;

            public Handler(BussyDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<OrderModel> Handle(AddOrderCommand request, CancellationToken cancellationToken)
            {
                var order = _mapper.Map<Order> (request.OrderToAdd);
                _db.Orders.Add(order);

                await _db.SaveChangesAsync(cancellationToken);

                return await _db.Orders
                    .AsNoTracking()
                    .ProjectTo<OrderModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(o => o.Id == order.Id, cancellationToken);
            }
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Bussy.Server.Databases;
using Bussy.Server.Exceptions;
using Bussy.Server.Models.Order;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bussy.Server.Domain.Orders.Features
{
    public static class UpdateOrder
    {
        public class UpdateOrderCommand : IRequest<bool>
        {
            public Guid Id { get; set; }
            public OrderModelForUpdate OrderToUpdate { get; set; }

            public UpdateOrderCommand(Guid Order, OrderModelForUpdate newOrderData)
            {
                Id = Order;
                OrderToUpdate = newOrderData;
            }
        }

        public class Handler : IRequestHandler<UpdateOrderCommand, bool>
        {
            private readonly BussyDbContext _db;
            private readonly IMapper _mapper;

            public Handler(BussyDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<bool> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
            {
                var OrderToUpdate = await _db.Orders
                    .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

                if (OrderToUpdate == null)
                    throw new NotFoundException("Order", request.Id);

                _mapper.Map(request.OrderToUpdate, OrderToUpdate);

                await _db.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
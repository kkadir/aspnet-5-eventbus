using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bussy.Server.Databases;
using Bussy.Server.Exceptions;
using Bussy.Server.Models.Order;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bussy.Server.Domain.Orders.Features
{
    public static class GetOrder
    {
        public class OrderQuery : IRequest<OrderModel>
        {
            public Guid Id { get; set; }

            public OrderQuery(Guid id)
            {
                Id = id;
            }
        }

        public class Handler : IRequestHandler<OrderQuery, OrderModel>
        {
            private readonly BussyDbContext _db;
            private readonly IMapper _mapper;

            public Handler(BussyDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<OrderModel> Handle(OrderQuery request, CancellationToken cancellationToken)
            {
                var result = await _db.Orders
                    .AsNoTracking()
                    .ProjectTo<OrderModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

                if (result == null)
                    throw new NotFoundException("Order", request.Id);

                return result;
            }
        }
    }
}
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
    public static class DeleteOrder
    {
        public class DeleteOrderCommand : IRequest<bool>
        {
            public Guid Id { get; set; }

            public DeleteOrderCommand(Guid Order)
            {
                Id = Order;
            }
        }

        public class Handler : IRequestHandler<DeleteOrderCommand, bool>
        {
            private readonly BussyDbContext _db;
            private readonly IMapper _mapper;

            public Handler(BussyDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
            {
                var recordToDelete = await _db.Orders
                    .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

                if (recordToDelete == null)
                    throw new NotFoundException("Order", request.Id);

                _db.Orders.Remove(recordToDelete);
                await _db.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
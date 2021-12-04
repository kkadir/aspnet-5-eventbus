using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Bussy.Server.Databases;
using Bussy.Server.Models.Order;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Bussy.Server.Domain.Orders.Validators
{
    public class OrderModelForManipulationValidator<T> : AbstractValidator<T> where T: OrderModelForManipulation
    {
        private readonly BussyDbContext _db;
        
        public OrderModelForManipulationValidator(BussyDbContext db)
        {
            _db = db;
            
            RuleFor(o => o)
                .MustAsync(BeUniqueOrder)
                .WithMessage(
                    "The order must be unique. An order with the same {symbol}-{price}-{size} combination for " +
                    "the same account already exists!");

            RuleFor(o => o.Account)
                .NotEmpty().WithMessage("An order account number cannot be empty")
                .MaximumLength(16).WithMessage("An order account number cannot exceed 16-characters!");
            
            RuleFor(o => o.Symbol)
                .NotEmpty().WithMessage("An order symbol cannot be empty")
                .MaximumLength(5).WithMessage("An order symbol cannot exceed 5-characters!");

            RuleFor(o => o.Price)
                .NotEmpty().WithMessage("An order price cannot be empty")
                .LessThanOrEqualTo(0).WithMessage("An order price cannot be less than or equal to 0!");
                
            RuleFor(o => o.Size)
                .NotEmpty().WithMessage("An order size cannot be empty")
                .LessThanOrEqualTo(0).WithMessage("An order size be less than or equal to 0!");
        }

        private async Task<bool> BeUniqueOrder(T order, CancellationToken cancellationToken)
        {
            return await _db.Orders.AllAsync(o =>
                o.IsDeleted == false || 
                (o.Account != order.Account 
                 && o.Symbol != order.Symbol 
                 && Math.Abs(o.Price - order.Price) > 0.000000001 
                 && o.Size != order.Size),
                cancellationToken);
        }
    }
}
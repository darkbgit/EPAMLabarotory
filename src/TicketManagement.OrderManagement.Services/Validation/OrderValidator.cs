using FluentValidation;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.Core.OrderManagement.Services.Validation
{
    internal sealed class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(s => s)
                .NotNull();

            RuleFor(s => s.UserId)
                .NotEmpty()
                .WithMessage("Invalid UserId");

            RuleFor(s => s.EventSeatId)
                .NotEmpty()
                .WithMessage("Invalid EventSeat Id");

            RuleFor(s => s.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Invalid Price");

            RuleFor(s => s.BoughtDate)
                .NotEmpty()
                .WithMessage("Invalid Date");
        }
    }
}
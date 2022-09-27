using FluentValidation;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.Core.EventManagement.Services.Validation
{
    internal sealed class EventAreaValidator : AbstractValidator<EventArea>
    {
        public EventAreaValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(a => a)
                .NotNull();

            RuleFor(a => a.Description)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Invalid Description");

            RuleFor(a => a.EventId)
                .GreaterThan(0)
                .WithMessage("Invalid EventId");

            RuleFor(a => a.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Invalid Price");
        }
    }
}
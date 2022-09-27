using FluentValidation;
using TicketManagement.BusinessLogic.Task1.Models;

namespace TicketManagement.BusinessLogic.Task1.Validation
{
    internal class EventAreaValidator : AbstractValidator<EventAreaDto>
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
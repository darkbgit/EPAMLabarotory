using FluentValidation;
using TicketManagement.BusinessLogic.Task1.Models;

namespace TicketManagement.BusinessLogic.Task1.Validation
{
    internal class EventSeatValidator : AbstractValidator<EventSeatDto>
    {
        public EventSeatValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(s => s)
                .NotNull();

            RuleFor(s => s.EventAreaId)
                .GreaterThan(0)
                .WithMessage("Invalid AreaId");

            RuleFor(s => s.Row)
                .GreaterThan(0)
                .WithMessage("Invalid Row");

            RuleFor(s => s.Number)
                .GreaterThan(0)
                .WithMessage("Invalid Number");
        }
    }
}
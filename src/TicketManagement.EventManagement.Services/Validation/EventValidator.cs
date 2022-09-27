using FluentValidation;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.Core.EventManagement.Services.Validation
{
    internal sealed class EventValidator : AbstractValidator<Event>
    {
        public EventValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(e => e)
                .NotNull();

            RuleFor(e => e.Description)
                .NotEmpty()
                .WithMessage("Invalid Description");

            RuleFor(e => e.Name)
                .NotEmpty()
                .MaximumLength(120)
                .WithMessage("Invalid Name");

            RuleFor(e => e.LayoutId)
                .GreaterThan(0)
                .WithMessage("Invalid LayoutId");

            RuleFor(e => e)
                .Must(e => e.StartDate < e.EndDate)
                .WithMessage("StartDate must be lower then EndDate.");
        }
    }
}
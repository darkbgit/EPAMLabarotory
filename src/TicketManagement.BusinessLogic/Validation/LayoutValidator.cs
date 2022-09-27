using FluentValidation;
using TicketManagement.BusinessLogic.Task1.Models;

namespace TicketManagement.BusinessLogic.Task1.Validation
{
    internal class LayoutValidator : AbstractValidator<LayoutDto>
    {
        public LayoutValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(l => l)
                .NotNull();

            RuleFor(l => l.Description)
                .NotEmpty()
                .MaximumLength(120)
                .WithMessage("Invalid Description");

            RuleFor(l => l.VenueId)
                .GreaterThan(0)
                .WithMessage("Invalid VenueId");
        }
    }
}
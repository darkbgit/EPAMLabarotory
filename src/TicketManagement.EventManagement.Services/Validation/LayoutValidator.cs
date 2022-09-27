using FluentValidation;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.Core.EventManagement.Services.Validation
{
    internal sealed class LayoutValidator : AbstractValidator<Layout>
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
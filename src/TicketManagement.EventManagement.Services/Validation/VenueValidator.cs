using FluentValidation;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.Core.EventManagement.Services.Validation
{
    internal sealed class VenueValidator : AbstractValidator<Venue>
    {
        public VenueValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(v => v)
                .NotNull();

            RuleFor(v => v.Description)
                .NotEmpty()
                .MaximumLength(120)
                .WithMessage("Invalid Description");

            RuleFor(v => v.Address)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Invalid Address");

            RuleFor(v => v.Phone)
                .MaximumLength(30)
                .WithMessage("Invalid Phone");
        }
    }
}
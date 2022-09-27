using FluentValidation;
using TicketManagement.BusinessLogic.Task1.Models;

namespace TicketManagement.BusinessLogic.Task1.Validation
{
    internal class VenueValidator : AbstractValidator<VenueDto>
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
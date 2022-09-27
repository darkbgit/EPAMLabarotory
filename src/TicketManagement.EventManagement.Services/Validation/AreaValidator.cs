using FluentValidation;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.Core.EventManagement.Services.Validation
{
    internal sealed class AreaValidator : AbstractValidator<Area>
    {
        public AreaValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(a => a)
                .NotNull();

            RuleFor(a => a.Description)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Invalid Description");

            RuleFor(a => a.LayoutId)
                .GreaterThan(0)
                .WithMessage("Invalid LayoutId");
        }
    }
}
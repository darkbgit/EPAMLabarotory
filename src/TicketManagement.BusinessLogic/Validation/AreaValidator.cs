using FluentValidation;
using TicketManagement.BusinessLogic.Task1.Models;

namespace TicketManagement.BusinessLogic.Task1.Validation
{
    internal class AreaValidator : AbstractValidator<AreaDto>
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
using FluentValidation;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.Core.EventManagement.Services.Validation
{
    internal sealed class SeatValidator : AbstractValidator<Seat>
    {
        public SeatValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(s => s)
                .NotNull();

            RuleFor(s => s.AreaId)
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
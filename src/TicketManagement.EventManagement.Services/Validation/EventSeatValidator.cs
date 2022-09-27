using FluentValidation;
using TicketManagement.Core.Public.Enums;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.Core.EventManagement.Services.Validation
{
    internal sealed class EventSeatValidator : AbstractValidator<EventSeat>
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

            RuleFor(s => s.State)
                .Must(s =>
                    Enum.GetValues(typeof(SeatState))
                        .Cast<SeatState>()
                        .Select(e => (int)e)
                        .ToList()
                        .Contains(s))
                .WithMessage("Invalid SeatState");
        }
    }
}
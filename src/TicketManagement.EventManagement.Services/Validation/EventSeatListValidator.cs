using FluentValidation;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.Core.EventManagement.Services.Validation
{
    internal sealed class EventSeatListValidator : AbstractValidator<IEnumerable<EventSeat>>
    {
        public EventSeatListValidator()
        {
            RuleForEach(s => s).SetValidator(new EventSeatValidator());

            RuleForEach(s => s)
                .Must((model, submodel) =>
                    model.Count(xsub => xsub.Row == submodel.Row && xsub.Number == submodel.Number) == 1)
                .WithMessage("The seat with Row and Number has duplicates in collection of items");
        }
    }
}
using FluentValidation;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.Core.EventManagement.Services.Validation
{
    internal class EventServiceValidator
    {
        public EventServiceValidator(IValidator<Event> eventValidator,
            IValidator<EventArea> eventAreaValidator,
            IValidator<IEnumerable<EventSeat>> eventSeatListValidator)
        {
            EventValidator = eventValidator;
            EventAreaValidator = eventAreaValidator;
            EventSeatListValidator = eventSeatListValidator;
        }

        public IValidator<Event> EventValidator { get; }
        public IValidator<EventArea> EventAreaValidator { get; }
        public IValidator<IEnumerable<EventSeat>> EventSeatListValidator { get; }
    }
}

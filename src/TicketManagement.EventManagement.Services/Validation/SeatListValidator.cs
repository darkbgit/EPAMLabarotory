using FluentValidation;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.Core.EventManagement.Services.Validation
{
    internal sealed class SeatListValidator : AbstractValidator<IEnumerable<Seat>>
    {
        public SeatListValidator()
        {
            RuleForEach(s => s).SetValidator(new SeatValidator());

            RuleForEach(s => s)
                .Must((model, submodel) =>
                    model.Count(xsub => xsub.Row == submodel.Row && xsub.Number == submodel.Number) == 1)
                .WithMessage("The seat with Row and Number has duplicates in collection of items");
        }
    }
}
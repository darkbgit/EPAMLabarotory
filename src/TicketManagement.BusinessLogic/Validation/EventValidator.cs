using System;
using FluentValidation;
using TicketManagement.BusinessLogic.Task1.Models;

namespace TicketManagement.BusinessLogic.Task1.Validation
{
    internal class EventValidator : AbstractValidator<EventDto>
    {
        public EventValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(e => e)
                .NotNull();

            RuleFor(e => e.Description)
                .NotEmpty()
                .WithMessage("Invalid Description");

            RuleFor(e => e.Name)
                .NotEmpty()
                .MaximumLength(120)
                .WithMessage("Invalid Name");

            RuleFor(e => e.LayoutId)
                .GreaterThan(0)
                .WithMessage("Invalid LayoutId");

            RuleFor(e => e.StartDate)
                .GreaterThan(DateTime.Now)
                .WithMessage("Invalid StartDate");

            RuleFor(e => e.EndDate)
                .GreaterThan(e => e.StartDate)
                .WithMessage("Invalid EndDate");
        }
    }
}
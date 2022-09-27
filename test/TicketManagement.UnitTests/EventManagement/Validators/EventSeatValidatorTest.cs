using System;
using System.Linq;
using AutoFixture;
using FluentValidation.TestHelper;
using NUnit.Framework;
using TicketManagement.Core.EventManagement.Services.Validation;
using TicketManagement.Core.Public.Enums;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.UnitTests.Helpers;

namespace TicketManagement.UnitTests.EventManagement.Validators
{
    [TestFixture]
    internal class EventSeatValidatorTest
    {
        private EventSeatValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new EventSeatValidator();
        }

        [Test]
        public void BadSeat_WithoutAreaId_ExecuteWithException()
        {
            // Arrange
            var fixture = FixtureFactory.GetFixture();

            var model = fixture.Build<EventSeat>()
                .Without(m => m.EventAreaId)
                .Create();

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(seat => seat.EventAreaId);
        }

        [Test]
        public void BadSeat_NegativeRow_ExecuteWithException()
        {
            // Arrange
            var fixture = FixtureFactory.GetFixture();

            var model = fixture.Build<EventSeat>()
                .With(m => m.Row, -1)
                .Create();

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(seat => seat.Row);
        }

        [Test]
        public void BadSeat_NegativeNumber_ExecuteWithException()
        {
            // Arrange
            var fixture = FixtureFactory.GetFixture();

            var model = fixture.Build<EventSeat>()
                .With(m => m.Number, -1)
                .Create();

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(seat => seat.Number);
        }

        public void BadSeat_InvalidState_ExecuteWithException()
        {
            // Arrange
            var fixture = FixtureFactory.GetFixture();

            var model = fixture.Build<EventSeat>()
                .With(eventSeat =>
                    eventSeat.State, Enum.GetValues(typeof(SeatState)).Cast<int>().Max() + 1)
                .Create();

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(seat => seat.Number);
        }

        [Test]
        public void GoodArea_ExecuteWithoutException()
        {
            // Arrange
            var fixture = FixtureFactory.GetFixture();

            var model = fixture.Build<EventSeat>()
                .With(eventSeat => eventSeat.State, (int)SeatState.Free)
                .Create();

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

using AutoFixture;
using FluentValidation.TestHelper;
using NUnit.Framework;
using TicketManagement.Core.EventManagement.Services.Validation;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.UnitTests.Helpers;

namespace TicketManagement.UnitTests.EventManagement.Validators
{
    [TestFixture]
    internal class EventAreaValidatorTest
    {
        private EventAreaValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new EventAreaValidator();
        }

        [Test]
        public void BadArea_WithoutDescription_ExecuteWithException()
        {
            // Arrange
            var fixture = FixtureFactory.GetFixture();

            var model = fixture.Build<EventArea>()
                .Without(m => m.Description)
                .Create();

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(m => m.Description);
        }

        [Test]
        public void BadArea_NegativePrice_ExecuteWithException()
        {
            // Arrange
            var fixture = FixtureFactory.GetFixture();

            var model = fixture.Build<EventArea>()
                .With(m => m.Price, -1)
                .Create();

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(m => m.Price);
        }

        [Test]
        public void GoodArea_ExecuteWithoutException()
        {
            // Arrange
            var fixture = FixtureFactory.GetFixture();

            var model = fixture.Build<EventArea>()
                .Create();

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

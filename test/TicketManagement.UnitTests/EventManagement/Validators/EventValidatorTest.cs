using System;
using AutoFixture;
using FluentValidation.TestHelper;
using NUnit.Framework;
using TicketManagement.Core.EventManagement.Services.Validation;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.UnitTests.Helpers;

namespace TicketManagement.UnitTests.EventManagement.Validators
{
    [TestFixture]
    internal class EventValidatorTest
    {
        private EventValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new EventValidator();
        }

        [Test]
        public void BadEventArea_WithoutDescription_ExecuteWithException()
        {
            // Arrange
            var fixture = FixtureFactory.GetFixture();

            var model = fixture.Build<Event>()
                .Without(m => m.Description)
                .Create();

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(m => m.Description);
        }

        [Test]
        public void BadEvent_WithoutName_ExecuteWithException()
        {
            // Arrange
            var fixture = FixtureFactory.GetFixture();

            var model = fixture.Build<Event>()
                .Without(m => m.Name)
                .Create();

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(m => m.Name);
        }

        [Test]
        public void BadEvent_WithoutLayoutId_ExecuteWithException()
        {
            // Arrange
            var fixture = FixtureFactory.GetFixture();

            var model = fixture.Build<Event>()
                .Without(m => m.LayoutId)
                .Create();

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(m => m.LayoutId);
        }

        [Test]
        public void BadEvent_EndDateBeforeStartDate_ExecuteWithException()
        {
            // Arrange
            var fixture = FixtureFactory.GetFixture();

            var model = fixture.Build<Event>()
                .With(m => m.StartDate, DateTime.Now.AddDays(3))
                .With(m => m.EndDate, DateTime.Now.AddDays(2))
                .Create();

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveAnyValidationError();
        }

        [Test]
        public void GoodArea_ExecuteWithoutException()
        {
            // Arrange
            var fixture = FixtureFactory.GetFixture();

            var model = fixture.Build<Event>()
                .With(m => m.StartDate, DateTime.Now.AddDays(1))
                .With(m => m.EndDate, DateTime.Now.AddDays(2))
                .Create();

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

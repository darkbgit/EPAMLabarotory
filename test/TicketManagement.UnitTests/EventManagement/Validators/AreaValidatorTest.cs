using AutoFixture;
using FluentValidation.TestHelper;
using NUnit.Framework;
using TicketManagement.Core.EventManagement.Services.Validation;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.UnitTests.Helpers;

namespace TicketManagement.UnitTests.EventManagement.Validators
{
    [TestFixture]
    internal class AreaValidatorTest
    {
        private AreaValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new AreaValidator();
        }

        [Test]
        public void BadArea_WithoutDescription_ExecuteWithException()
        {
            // Arrange
            var fixture = FixtureFactory.GetFixture();

            var model = fixture.Build<Area>()
                .Without(m => m.Description)
                .Create();

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(area => area.Description);
        }

        [Test]
        public void GoodArea_ExecuteWithoutException()
        {
            // Arrange
            var fixture = FixtureFactory.GetFixture();

            var model = fixture.Build<Area>()
                .Create();

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

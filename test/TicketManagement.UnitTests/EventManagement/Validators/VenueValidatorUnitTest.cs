using AutoFixture;
using FluentValidation.TestHelper;
using NUnit.Framework;
using TicketManagement.Core.EventManagement.Services.Validation;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.UnitTests.Helpers;

namespace TicketManagement.UnitTests.EventManagement.Validators
{
    [TestFixture]
    internal class VenueValidatorUnitTest
    {
        private VenueValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new VenueValidator();
        }

        [Test]
        public void BadVenue_WithoutDescription_ExecuteWithException()
        {
            // Arrange
            var fixture = FixtureFactory.GetFixture();

            var model = fixture.Build<Venue>()
                .Without(m => m.Description)
                .Create();

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(venue => venue.Description);
        }

        [Test]
        public void GoodVenue_ExecuteWithoutException()
        {
            // Arrange
            var fixture = FixtureFactory.GetFixture();

            var model = fixture.Build<Venue>()
                .With(v => v.Phone, fixture.Create<string>()[..30])
                .Create();

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

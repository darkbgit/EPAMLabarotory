using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TicketManagement.Core.EventManagement.Services.Interfaces;
using TicketManagement.Core.Public.DTOs.EventAreaDTOs;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.Core.Public.Requests;
using TicketManagement.EventManagement.API.Controllers;
using TicketManagement.UnitTests.Helpers;

namespace TicketManagement.UnitTests.EventManagement.API
{
    public class EventAreasControllerTest
    {
        private readonly Mock<IEventAreaService> _eventAreaServiceMock = new ();
        private EventAreasController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new EventAreasController(_eventAreaServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _eventAreaServiceMock.Reset();
        }

        [Test]
        public async Task GetEventAreas_ReturnsOkResult()
        {
            //Arrange
            var fixture = FixtureFactory.GetFixture();

            var id = fixture.Create<int>();

            var request = fixture.Create<PaginationRequest>();

            var response = fixture.Create<PaginatedList<EventAreaWithSeatsNumberDto>>();

            _eventAreaServiceMock
                .Setup(s => s.GetPagedEventAreasByEventIdAsync(id, request.PageIndex.Value, request.PageSize, It.IsAny<CancellationToken>() ))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetEventAreasForEditList(id, request);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task GetEventAreaById_ReturnsOkResult()
        {
            //Arrange
            var fixture = FixtureFactory.GetFixture();

            var id = fixture.Create<int>();

            var response = fixture.Create<EventAreaDto>();

            _eventAreaServiceMock
                .Setup(s => s.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetEventAreaById(id);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            (result.Result as OkObjectResult)?.Value.Should().BeEquivalentTo(response);
        }

        [Test]
        public async Task GetEventAreaById_ReturnsNotFoundResult()
        {
            //Arrange
            var fixture = FixtureFactory.GetFixture();

            var id = fixture.Create<int>();

            EventAreaDto response = null;

            _eventAreaServiceMock
                .Setup(s => s.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetEventAreaById(id);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task GetEventAreasForEditList_ReturnsOkResult()
        {
            //Arrange
            var fixture = FixtureFactory.GetFixture();

            var eventId = fixture.Create<int>();

            var request = fixture.Create<PaginationRequest>();

            var response = fixture.Create<PaginatedList<EventAreaWithSeatsNumberDto>>();

            _eventAreaServiceMock
                .Setup(eventAreaService => eventAreaService
                    .GetPagedEventAreasByEventIdAsync(eventId, request.PageIndex.Value, request.PageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetEventAreasForEditList(eventId, request);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            (result.Result as OkObjectResult)?.Value.Should().BeEquivalentTo(response);
        }

        [Test]
        public async Task GetEventAreasWithSeatsInfoByEventId_ReturnsOkResult()
        {
            //Arrange
            var fixture = FixtureFactory.GetFixture();

            var eventId = fixture.Create<int>();

            var response = fixture.CreateMany<EventAreaWithSeatsAndFreeSeatsCountDto>().ToList();

            _eventAreaServiceMock
                .Setup(eventAreaService => eventAreaService
                    .GetEventAreasWithSeatsInfoByEventIdAsync(eventId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetEventAreasWithSeatsInfoByEventId(eventId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            (result.Result as OkObjectResult)?.Value.Should().BeEquivalentTo(response);
        }

        [Test]
        public void DeleteEventArea_NotExistingIdPassed_ReturnsNotFoundResponse()
        {
            // Arrange
            var fixture = FixtureFactory.GetFixture();

            var notExistingId = fixture.Create<int>();

            EventAreaDto response = null;

            _eventAreaServiceMock
                .Setup(eventAreaService => eventAreaService
                    .GetByIdAsync(notExistingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = _controller.DeleteEventArea(notExistingId);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public void DeleteEventArea_ExistingIdPassed_ReturnsNoContentResult()
        {
            // Arrange
            var fixture = FixtureFactory.GetFixture();

            var eventAreaId = fixture.Create<int>();

            var response = fixture.Create<EventAreaDto>();

            _eventAreaServiceMock
                .Setup(eventAreaService => eventAreaService
                    .GetByIdAsync(eventAreaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            _eventAreaServiceMock
                .Setup(eventAreaService => eventAreaService
                    .DeleteAsync(eventAreaId, It.IsAny<CancellationToken>()))
                .Verifiable();

            // Act
            var result = _controller.DeleteEventArea(eventAreaId);

            // Assert
            result.Result.Should().BeOfType<NoContentResult>();
        }
    }
}
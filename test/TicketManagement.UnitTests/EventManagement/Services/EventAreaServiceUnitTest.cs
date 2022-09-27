using System;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using TicketManagement.Core.EventManagement.Services;
using TicketManagement.Core.EventManagement.Services.Interfaces;
using TicketManagement.Core.EventManagement.Services.Validation;
using TicketManagement.Core.Public.DTOs.EventAreaDTOs;
using TicketManagement.Core.Public.Enums;
using TicketManagement.Core.Public.Exceptions;
using TicketManagement.Core.Public.Localization;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.Public;
using TicketManagement.UnitTests.Helpers;

namespace TicketManagement.UnitTests.EventManagement.Services;

internal class EventAreaServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new ();
    private readonly Mock<IMapper> _mapperMock = new ();
    private readonly Mock<ILogger<EventAreaService>> _loggerMock = new ();
    private readonly IValidator<EventArea> _eventAreaValidator = new EventAreaValidator();
    private readonly CancellationToken _cancellationToken = default;
    private IStringLocalizer<SharedResource> _serviceResourcesStringLocalizer;
    private IEventAreaService _eventAreaService;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _mapperMock
            .Setup(mapper => mapper.Map<EventArea>(It.IsAny<EventAreaDto>()))
            .Returns((EventAreaDto dto) =>
            {
                var entity = new EventArea
                {
                    Id = dto.Id,
                    Description = dto.Description,
                    EventId = dto.EventId,
                    CoordX = dto.CoordX,
                    CoordY = dto.CoordY,
                    Price = dto.Price,
                };

                return entity;
            });

        _mapperMock
            .Setup(mapper => mapper.Map<EventAreaDto>(It.IsAny<EventArea>()))
            .Returns((EventArea entity) =>
            {
                var dto = new EventAreaDto
                {
                    Id = entity.Id,
                    Description = entity.Description,
                    EventId = entity.EventId,
                    CoordX = entity.CoordX,
                    CoordY = entity.CoordY,
                    Price = entity.Price,
                };

                return dto;
            });

        _mapperMock
            .Setup(mapper => mapper.Map<EventArea>(It.IsAny<EventAreaWithSeatsAndFreeSeatsCountDto>()))
            .Returns((EventAreaWithSeatsAndFreeSeatsCountDto entity) =>
            {
                var dto = new EventArea
                {
                    Id = entity.Id,
                    Description = entity.Description,
                    EventId = entity.EventId,
                    CoordX = entity.CoordX,
                    CoordY = entity.CoordY,
                    Price = entity.Price,
                };

                return dto;
            });

        _mapperMock
            .Setup(mapper => mapper.Map<EventAreaDto>(null))
            .Returns(() =>
            {
                EventAreaDto result = null;
                return result;
            });

        var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
        var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
        _serviceResourcesStringLocalizer = new StringLocalizer<SharedResource>(factory);
    }

    [SetUp]
    public void Setup()
    {
        _eventAreaService = new EventAreaService(_unitOfWorkMock.Object,
            _mapperMock.Object,
            _eventAreaValidator,
            _loggerMock.Object,
            _serviceResourcesStringLocalizer);
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWorkMock.Reset();
    }

    [Test]
    public void UpdateEventArea_GivenValidEventArea_ExecutesWithoutException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventAreaDto = fixture.Build<EventAreaDto>()
            .Create();

        var eventArea = _mapperMock.Object.Map<EventArea>(eventAreaDto);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea.GetByIdAsync(eventArea.Id, _cancellationToken))
            .ReturnsAsync(eventArea);

        _unitOfWorkMock
            .Setup(unitOfWork =>
                unitOfWork.EventArea.IsAnotherExistAsync(It.Is<EventArea>(eventArea =>
                    eventArea.Description == eventAreaDto.Description),
                    _cancellationToken))
            .ReturnsAsync(false);

        // Act, Assert
        Assert.DoesNotThrowAsync(async () => await _eventAreaService.UpdateAsync(eventAreaDto, _cancellationToken));
    }

    [Test]
    public void UpdateEventArea_EventAreaNotFound_ExecutesWithException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventAreaDto = fixture.Build<EventAreaDto>()
            .Create();

        EventArea eventArea = null;

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea.GetByIdAsync(eventAreaDto.Id, _cancellationToken))
            .ReturnsAsync(eventArea);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _eventAreaService.UpdateAsync(eventAreaDto, _cancellationToken));
    }

    [Test]
    public void UpdateEventArea_GivenEventAreaWithExistedDescription_ExecutesWithException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventAreaDto = fixture.Build<EventAreaDto>()
            .Create();

        var eventArea = _mapperMock.Object.Map<EventArea>(eventAreaDto);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea.GetByIdAsync(eventArea.Id, _cancellationToken))
            .ReturnsAsync(eventArea);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea.IsAnotherExistAsync(It.Is<EventArea>(v => v.Description == eventAreaDto.Description), _cancellationToken))
            .ReturnsAsync(true);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _eventAreaService.UpdateAsync(eventAreaDto, _cancellationToken));
    }

    [Test]
    public void UpdateEventArea_EventAreaRepositoryThrowDbException_ExecutesWithException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventAreaDto = fixture.Build<EventAreaDto>()
            .Create();

        var eventArea = _mapperMock.Object.Map<EventArea>(eventAreaDto);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea.GetByIdAsync(eventArea.Id, _cancellationToken))
            .ReturnsAsync(eventArea);

        _unitOfWorkMock
            .Setup(unitOfWork =>
                unitOfWork.EventArea.IsExistAsync(It.Is<EventArea>(eventArea =>
                        eventArea.Description == eventAreaDto.Description),
                    _cancellationToken))
            .ReturnsAsync(false);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(_cancellationToken))
            .ThrowsAsync(new Mock<DbException>().Object);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _eventAreaService.UpdateAsync(eventAreaDto, _cancellationToken));
    }

    [Test]
    public void DeleteEventArea_ExecutesWithoutException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventArea = fixture.Create<EventArea>();

        var eventSeats = Enumerable.Empty<EventSeat>();

        var eventSeatsQuery = eventSeats
            .BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventSeat.GetEventSeatsByEventAreaId(eventArea.Id))
            .Returns(eventSeatsQuery);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea.GetByIdAsync(eventArea.Id, _cancellationToken))
            .ReturnsAsync(eventArea);

        // Act, Assert
        Assert.DoesNotThrowAsync(async () => await _eventAreaService.DeleteAsync(eventArea.Id, _cancellationToken));
    }

    [Test]
    public void DeleteEventArea_EventAreaRepositoryThrowDbException_ExecutesWithException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventArea = fixture.Create<EventArea>();
        var eventSeats = Enumerable.Empty<EventSeat>();

        var eventSeatsQuery = eventSeats
            .BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventSeat.GetEventSeatsByEventAreaId(eventArea.Id))
            .Returns(eventSeatsQuery);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea.GetByIdAsync(eventArea.Id, _cancellationToken))
            .ReturnsAsync(eventArea);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(_cancellationToken))
            .ThrowsAsync(new Mock<DbException>().Object);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _eventAreaService.DeleteAsync(eventArea.Id, _cancellationToken));
    }

    [Test]
    public void DeleteEventArea_EventAreaNotFound_ExecutesWithException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventAreaId = fixture.Create<int>();

        EventArea eventArea = null;

        var eventSeats = Enumerable.Empty<EventSeat>();

        var eventSeatsQuery = eventSeats
            .BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventSeat.GetEventSeatsByEventAreaId(eventAreaId))
            .Returns(eventSeatsQuery);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea.GetByIdAsync(eventAreaId, _cancellationToken))
            .ReturnsAsync(eventArea);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _eventAreaService.DeleteAsync(eventAreaId, _cancellationToken));
    }

    [Test]
    public void DeleteEventArea_EventAreaWithAnySeatOccupied_ExecutesWithException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventArea = fixture.Create<EventArea>();

        var eventSeats = fixture.Build<EventSeat>()
            .With(eventSeat => eventSeat.State, (int)SeatState.Occupied)
            .CreateMany();

        var eventSeatsQuery = eventSeats
            .BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventSeat.GetEventSeatsByEventAreaId(eventArea.Id))
            .Returns(eventSeatsQuery);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea.GetByIdAsync(eventArea.Id, _cancellationToken))
            .ReturnsAsync(eventArea);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _eventAreaService.DeleteAsync(eventArea.Id, _cancellationToken));
    }

    [Test]
    public async Task GetById_GivenValidId_ReturnsEventAreaDto()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventAreaDto = fixture.Build<EventAreaDto>()
            .Create();

        var eventArea = _mapperMock.Object.Map<EventArea>(eventAreaDto);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea.GetByIdAsync(eventArea.Id, _cancellationToken))
            .ReturnsAsync(eventArea);

        // Act
        var result = await _eventAreaService.GetByIdAsync(eventArea.Id, _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(eventAreaDto);
    }

    [Test]
    public async Task GetById_GivenInvalidId_ReturnsNull()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventAreaId = fixture.Create<int>();

        EventArea eventArea = null;

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea.GetByIdAsync(eventAreaId, _cancellationToken))
            .ReturnsAsync(eventArea);

        // Act
        var result = await _eventAreaService.GetByIdAsync(eventAreaId, _cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GetEventAreasByEventId_GivenValidId_ReturnsIEnumerableEventAreaDto()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventId = fixture.Create<int>();

        var eventAreas = fixture.Build<EventAreaWithSeatsAndFreeSeatsCountDto>()
            .With(e => e.EventId, eventId)
            .CreateMany();

        var eventAreasQuery = eventAreas.BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea
                .GetEventAreasWithSeatsAndFreeSeatsCountByEventId(eventId))
            .Returns(eventAreasQuery);

        // Act
        var result = await _eventAreaService.GetEventAreasWithSeatsInfoByEventIdAsync(eventId, _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(eventAreas);
    }

    [Test]
    public async Task GetEventAreasByEventId_GivenInvalidId_ReturnsEmptyIEnumerable()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventId = fixture.Create<int>();

        var eventAreas = Enumerable.Empty<EventAreaWithSeatsAndFreeSeatsCountDto>();

        var eventAreasQuery = eventAreas.BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea
                .GetEventAreasWithSeatsAndFreeSeatsCountByEventId(eventId))
            .Returns(eventAreasQuery);

        // Act
        var result = await _eventAreaService.GetEventAreasWithSeatsInfoByEventIdAsync(eventId, _cancellationToken);

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetPagedEventAreasByEventId_GivenValidId_ReturnsIEnumerableEventAreaDto()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventId = fixture.Create<int>();

        var totalNumber = 10;

        var perPage = 5;

        var page = 1;

        var eventAreasDto = fixture.Build<EventAreaWithSeatsNumberDto>()
            .With(e => e.EventId, eventId)
            .CreateMany(totalNumber)
            .ToList();

        var paginatedEventAreas = eventAreasDto
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToList();

        var eventAreasDtoQuery = eventAreasDto.BuildMock();

        var eventAreas = fixture.Build<EventArea>()
            .With(e => e.EventId, eventId)
            .CreateMany(totalNumber)
            .ToList();

        var eventAreasQuery = eventAreas.BuildMock();

        var paginatedList = new PaginatedList<EventAreaWithSeatsNumberDto>(paginatedEventAreas,
            totalNumber,
            page,
            perPage);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea
                .GetEventAreasWithSeatsCountByEventId(eventId))
            .Returns(eventAreasDtoQuery);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea
                .FindBy(It.IsAny<Expression<Func<EventArea, bool>>>()))
            .Returns(eventAreasQuery);

        // Act
        var result = await _eventAreaService.GetPagedEventAreasByEventIdAsync(eventId, page, perPage, _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(paginatedList);
    }

    [Test]
    public async Task GetEventIdByEvenAreaId_GivenValidId_ReturnsEventId()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventId = fixture.Create<int>();

        var eventAreaId = fixture.Create<int>();

        var eventArea = fixture.Build<EventArea>()
            .With(eventArea => eventArea.Id, eventAreaId)
            .With(eventArea => eventArea.EventId, eventId)
            .CreateMany(1)
            .ToList();

        var eventAreaQuery = eventArea.BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea.FindBy(It.IsAny<Expression<Func<EventArea, bool>>>()))
            .Returns(eventAreaQuery);

        // Act
        var result = await _eventAreaService.GetEventIdByEventAreaIdAsync(eventAreaId, _cancellationToken);

        // Assert
        result.Should().Be(eventId);
    }

    [Test]
    public void GetEventIdByEvenAreaId_GivenInvalidId_ThrowsException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventAreaId = fixture.Create<int>();

        var eventArea = Enumerable.Empty<EventArea>();

        var eventAreaQuery = eventArea.BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea.FindBy(It.IsAny<Expression<Func<EventArea, bool>>>()))
            .Returns(eventAreaQuery);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _eventAreaService.GetEventIdByEventAreaIdAsync(eventAreaId, _cancellationToken));
    }
}
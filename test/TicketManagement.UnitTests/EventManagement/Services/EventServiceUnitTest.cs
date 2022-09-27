using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using TicketManagement.Core.EventManagement.Services;
using TicketManagement.Core.EventManagement.Services.Interfaces;
using TicketManagement.Core.EventManagement.Services.Validation;
using TicketManagement.Core.Public.DTOs.EventDTOs;
using TicketManagement.Core.Public.Enums;
using TicketManagement.Core.Public.Exceptions;
using TicketManagement.Core.Public.Localization;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.Public;
using TicketManagement.UnitTests.Helpers;

namespace TicketManagement.UnitTests.EventManagement.Services;

internal class EventServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new ();
    private readonly Mock<IMapper> _mapperMock = new ();
    private readonly IValidator<Event> _eventValidator = new EventValidator();
    private readonly IValidator<EventArea> _eventAreaValidator = new EventAreaValidator();
    private readonly IValidator<IEnumerable<EventSeat>> _eventSeatListValidator = new EventSeatListValidator();
    private readonly CancellationToken _cancellationToken = default;
    private readonly Mock<IWebHostEnvironment> _hostingEnvironmentMock = new ();
    private readonly Mock<IFileSystem> _fileSystemMock = new ();
    private readonly Mock<IImageBase64Service> _imageServiceMock = new ();
    private EventServiceValidator _eventServiceValidator;
    private IStringLocalizer<SharedResource> _serviceResourcesStringLocalizer;
    private IStringLocalizer<EventService> _stringLocalizer;
    private EventService _eventService;
    private IConfiguration _configuration;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _mapperMock
            .Setup(mapper => mapper.Map<Event>(It.IsAny<EventDto>()))
            .Returns((EventDto dto) =>
            {
                var entity = new Event
                {
                    Id = dto.Id,
                    Description = dto.Description,
                    Name = dto.Name,
                    LayoutId = dto.LayoutId,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                };

                return entity;
            });

        _mapperMock
            .Setup(mapper => mapper.Map<EventDto>(It.IsAny<Event>()))
            .Returns((Event entity) =>
            {
                var dto = new EventDto
                {
                    Id = entity.Id,
                    Description = entity.Description,
                    Name = entity.Name,
                    LayoutId = entity.LayoutId,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                };

                return dto;
            });

        _mapperMock
            .Setup(mapper => mapper.Map<EventDto>(null))
            .Returns(() =>
            {
                EventDto result = null;
                return result;
            });

        _mapperMock
            .Setup(mapper => mapper.Map<Event>(It.IsAny<EventForCreateDto>()))
            .Returns((EventForCreateDto dto) =>
            {
                var entity = new Event
                {
                    Description = dto.Description,
                    Name = dto.Name,
                    LayoutId = dto.LayoutId,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    ImageUrl = dto.ImageUrl,
                };

                return entity;
            });

        _mapperMock
            .Setup(mapper => mapper.Map<Event>(It.IsAny<EventForUpdateDto>()))
            .Returns((EventForUpdateDto dto) =>
            {
                var entity = new Event
                {
                    Id = dto.Id,
                    Description = dto.Description,
                    Name = dto.Name,
                    LayoutId = dto.LayoutId,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    ImageUrl = dto.ImageUrl,
                };

                return entity;
            });

        _mapperMock
            .Setup(mapper => mapper.Map<EventArea>(It.IsAny<Area>()))
            .Returns((Area dto) =>
            {
                var entity = new EventArea
                {
                    Description = dto.Description,
                    CoordX = dto.CoordX,
                    CoordY = dto.CoordY,
                    Id = dto.Id,
                };

                return entity;
            });

        _mapperMock
            .Setup(mapper => mapper.Map<EventSeat>(It.IsAny<Seat>()))
            .Returns((Seat dto) =>
            {
                var entity = new EventSeat
                {
                    Row = dto.Row,
                    Number = dto.Number,
                    Id = dto.Id,
                };

                return entity;
            });

        var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
        var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
        _serviceResourcesStringLocalizer = new StringLocalizer<SharedResource>(factory);
        _stringLocalizer = new StringLocalizer<EventService>(factory);

        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, false)
            .AddEnvironmentVariables()
            .Build();

        _fileSystemMock.Setup(f => f.File.Delete(It.IsAny<string>())).Verifiable();

        _hostingEnvironmentMock
            .Setup(h => h.WebRootPath)
            .Returns(string.Empty);

        _imageServiceMock
            .Setup(s => s.SaveImgFromBase64(It.IsAny<string>()))
            .ReturnsAsync(string.Empty);

        _eventServiceValidator = new EventServiceValidator(_eventValidator, _eventAreaValidator, _eventSeatListValidator);
    }

    [SetUp]
    public void Setup()
    {
        _eventService = new EventService(_unitOfWorkMock.Object,
            _mapperMock.Object,
            _serviceResourcesStringLocalizer,
            _stringLocalizer,
            _hostingEnvironmentMock.Object,
            _imageServiceMock.Object,
            _eventServiceValidator);
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWorkMock.Reset();
    }

    [Test]
    public void CreateAsync_GivenValidEvent_ExecutesWithoutException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventForCreateDto = fixture.Build<EventForCreateDto>()
            .With(eventForCreateDto => eventForCreateDto.StartDate, DateTime.Now.AddHours(1))
            .With(forCreateDto => forCreateDto.EndDate, DateTime.Now.AddHours(2))
            .Create();

        var eventEntity = _mapperMock.Object.Map<Event>(eventForCreateDto);

        var eventId = fixture.Create<int>();

        var areas = fixture.Build<Area>()
                .With(a => a.LayoutId, eventForCreateDto.LayoutId)
                .CreateMany(1);

        var areasQuery = areas
            .BuildMock();

        var seats = fixture.Build<Seat>()
            .With(seat => seat.AreaId, areas.First().Id)
            .CreateMany()
            .AsEnumerable();

        var seatsQuery = seats
            .BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Layout
                .GetByIdAsync(eventForCreateDto.LayoutId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fixture.Create<Layout>());

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.IsExistAsync(It.Is<Event>(e => e.Description == eventEntity.Description),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Area
                .GetAreasByLayoutId(eventForCreateDto.LayoutId))
            .Returns(areasQuery);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea.IsExistAsync(It.Is<EventArea>(e => areas.Any(a => a.Id == e.Id)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Seat.GetSeatsByAreaId(It.Is<int>(i => areas.Any(a => a.Id == i))))
            .Returns(seatsQuery);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.CreateEvent(It.Is<Event>(e => e.Description == eventEntity.Description)))
            .Returns(eventId);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventSeat.AddEventSeatsRange(It.IsAny<IEnumerable<EventSeat>>()))
            .Verifiable();

        // Act, Assert
        Assert.DoesNotThrowAsync(async () => await _eventService.CreateAsync(eventForCreateDto, _cancellationToken));
    }

    [Test]
    public void CreateAsync_GivenEventWithExistingParams_TrowsServiceException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventForCreateDto = fixture.Build<EventForCreateDto>()
            .With(forCreateDto => forCreateDto.StartDate, DateTime.Now.AddHours(1))
            .With(forCreateDto => forCreateDto.EndDate, DateTime.Now.AddHours(2))
            .Create();

        var eventEntity = _mapperMock.Object.Map<Event>(eventForCreateDto);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.IsExistAsync(It.Is<Event>(e => e.Description == eventEntity.Description),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _eventService.CreateAsync(eventForCreateDto, _cancellationToken));
    }

    [Test]
    public void CreateAsync_ThrowsDbExceptiont_ExecutesWithException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventForCreateDto = fixture.Build<EventForCreateDto>()
            .With(eventForCreateDto => eventForCreateDto.StartDate, DateTime.Now.AddHours(1))
            .With(forCreateDto => forCreateDto.EndDate, DateTime.Now.AddHours(2))
            .Create();

        var eventEntity = _mapperMock.Object.Map<Event>(eventForCreateDto);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.IsExistAsync(It.Is<Event>(e => e.Description == eventEntity.Description),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.CreateEvent(It.Is<Event>(e => e.Description == eventEntity.Description)))
            .Throws(new Mock<DbException>().Object);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _eventService.CreateAsync(eventForCreateDto, _cancellationToken));
    }

    [Test]
    public void UpdateEvent_GivenValidEvent_ExecutesWithoutException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventDto = fixture.Build<EventForUpdateDto>()
            .With(dto => dto.StartDate, DateTime.Now.AddHours(1))
            .With(dto => dto.EndDate, DateTime.Now.AddHours(2))
            .Create();

        var @event = _mapperMock.Object.Map<Event>(eventDto);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.GetByIdAsync(@event.Id, _cancellationToken))
            .ReturnsAsync(@event);

        _unitOfWorkMock
            .Setup(unitOfWork =>
                unitOfWork.Event.IsExistForUpdateAsync(It.Is<Event>(eventSeat =>
                    eventSeat.Id == @event.Id),
                    _cancellationToken))
            .ReturnsAsync(false);

        // Act, Assert
        Assert.DoesNotThrowAsync(async () => await _eventService.UpdateAsync(eventDto, _cancellationToken));
    }

    [Test]
    public void UpdateEvent_GivenEventWithExistedParameters_ExecutesWithException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventDto = fixture.Build<EventForUpdateDto>()
            .With(dto => dto.StartDate, DateTime.Now.AddHours(1))
            .With(dto => dto.EndDate, DateTime.Now.AddHours(2))
            .Create();

        var @event = _mapperMock.Object.Map<Event>(eventDto);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.GetByIdAsync(@event.Id, _cancellationToken))
            .ReturnsAsync(@event);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.IsExistForUpdateAsync(It.Is<Event>(event1 => event1.Id == eventDto.Id), _cancellationToken))
            .ReturnsAsync(true);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _eventService.UpdateAsync(eventDto, _cancellationToken));
    }

    [Test]
    public void UpdateEvent_EventRepositoryThrowDbException_ExecutesWithException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventDto = fixture.Build<EventForUpdateDto>()
            .With(dto => dto.StartDate, DateTime.Now.AddHours(1))
            .With(dto => dto.EndDate, DateTime.Now.AddHours(2))
            .Create();

        var @event = _mapperMock.Object.Map<Event>(eventDto);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.GetByIdAsync(@event.Id, _cancellationToken))
            .ReturnsAsync(@event);

        _unitOfWorkMock
            .Setup(unitOfWork =>
                unitOfWork.Event.IsExistForUpdateAsync(It.Is<Event>(eventSeat =>
                        eventSeat.Id == eventDto.Id),
                    _cancellationToken))
            .ReturnsAsync(false);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.UpdateEvent(@event))
            .Throws(new Mock<DbUpdateConcurrencyException>().Object);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _eventService.UpdateAsync(eventDto, _cancellationToken));
    }

    [Test]
    public void UpdateEvent_EventNotFound_ExecutesWithException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventDto = fixture.Build<EventForUpdateDto>()
            .Create();

        Event @event = null;

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.GetByIdAsync(eventDto.Id, _cancellationToken))
            .ReturnsAsync(@event);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _eventService.UpdateAsync(eventDto, _cancellationToken));
    }

    [Test]
    public void DeleteEvent_ExecutesWithoutException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var @event = fixture.Create<Event>();

        var eventAreas = Enumerable.Empty<EventArea>();

        var eventAreasQuery = eventAreas
            .BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea.GetEventAreasByEventId(@event.Id))
            .Returns(eventAreasQuery);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.GetByIdAsync(@event.Id, _cancellationToken))
            .ReturnsAsync(@event);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.DeleteEvent(@event.Id))
            .Returns(1);

        // Act, Assert
        Assert.DoesNotThrowAsync(async () => await _eventService.DeleteAsync(@event.Id, _cancellationToken));
    }

    [Test]
    public void DeleteEvent_ThrowsDbException_ExecutesWithException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var @event = fixture.Create<Event>();

        var eventAreas = Enumerable.Empty<EventArea>();

        var eventAreasQuery = eventAreas
            .BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea.GetEventAreasByEventId(@event.Id))
            .Returns(eventAreasQuery);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.GetByIdAsync(@event.Id, _cancellationToken))
            .ReturnsAsync(@event);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.DeleteEvent(@event.Id))
            .Throws(new Mock<DbException>().Object);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _eventService.DeleteAsync(@event.Id, _cancellationToken));
    }

    [Test]
    public void DeleteEvent_EventWithAnyArea_ExecutesWithException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var @event = fixture.Create<Event>();

        var eventAreas = fixture.Build<EventArea>()
            .CreateMany(1);

        var eventAreasQuery = eventAreas
            .BuildMock();

        var eventSeats = fixture.Build<EventSeat>()
            .With(eventSeat => eventSeat.EventAreaId, eventAreas.First().Id)
            .With(eventSeat => eventSeat.State, (int)SeatState.Occupied)
            .CreateMany(1)
            .ToList();

        var eventSeatsQuery = eventSeats
            .BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventSeat
                .GetEventSeatsByEventAreaId(eventAreas.First().Id))
            .Returns(eventSeatsQuery);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventArea.GetEventAreasByEventId(@event.Id))
            .Returns(eventAreasQuery);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.GetByIdAsync(@event.Id, _cancellationToken))
            .ReturnsAsync(@event);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _eventService.DeleteAsync(@event.Id, _cancellationToken));
    }

    [Test]
    public void DeleteEvent_EventNotFound_ExecutesWithException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventId = fixture.Create<int>();

        Event @event = null;

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.GetByIdAsync(eventId, _cancellationToken))
            .ReturnsAsync(@event);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _eventService.DeleteAsync(eventId, _cancellationToken));
    }

    [Test]
    public async Task GetPagedUpcomingEvents_ReturnsIEnumerableEventForListDto()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var perPage = 5;

        var page = 1;

        var totalEvents = 10;

        var events = fixture.Build<EventForListDto>()
            .CreateMany(totalEvents)
            .ToList();

        var eventsQuery = events.BuildMock();

        var paginatedEventsDto = events
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToList();

        var paginatedList = new PaginatedList<EventForListDto>(paginatedEventsDto,
            totalEvents,
            page,
            perPage);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.GetEventsForMainPage())
            .Returns(eventsQuery);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.EventsForManePageCountAsync(_cancellationToken))
            .ReturnsAsync(totalEvents);

        // Act
        var result = await _eventService.GetPagedUpcomingEventsAsync(page, perPage, _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(paginatedList);
    }

    [Test]
    [TestCase(ModeratorPanelEventListSortOrder.Name)]
    [TestCase(ModeratorPanelEventListSortOrder.NameDesc)]
    [TestCase(ModeratorPanelEventListSortOrder.VenueName)]
    [TestCase(ModeratorPanelEventListSortOrder.VenueNameDesc)]
    [TestCase(ModeratorPanelEventListSortOrder.LayoutName)]
    [TestCase(ModeratorPanelEventListSortOrder.LayoutNameDesc)]
    [TestCase(ModeratorPanelEventListSortOrder.StartDate)]
    [TestCase(ModeratorPanelEventListSortOrder.StartDateDesc)]
    [TestCase(ModeratorPanelEventListSortOrder.Duration)]
    [TestCase(ModeratorPanelEventListSortOrder.DurationDesc)]

    public async Task GetPagedEvents_ReturnsIEnumerableEventForModeratorListDto(ModeratorPanelEventListSortOrder sortOrder)
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var totalNumber = 10;

        var perPage = 5;

        var page = 1;

        var searchString = string.Empty;

        var events = fixture.Build<EventForEditListDto>()
            .CreateMany(totalNumber)
            .ToList();

        var eventsQuery = events.BuildMock();

        var sortedEvents = events = sortOrder switch
        {
            ModeratorPanelEventListSortOrder.VenueNameDesc => events.OrderByDescending(q => q.VenueDescription).ToList(),
            ModeratorPanelEventListSortOrder.VenueName => events.OrderBy(q => q.VenueDescription).ToList(),
            ModeratorPanelEventListSortOrder.LayoutNameDesc => events.OrderByDescending(q => q.LayoutDescription).ToList(),
            ModeratorPanelEventListSortOrder.LayoutName => events.OrderBy(q => q.LayoutDescription).ToList(),
            ModeratorPanelEventListSortOrder.StartDateDesc => events.OrderByDescending(q => q.StartDate).ToList(),
            ModeratorPanelEventListSortOrder.StartDate => events.OrderBy(q => q.StartDate).ToList(),
            ModeratorPanelEventListSortOrder.DurationDesc => events.OrderByDescending(q => q.Duration).ToList(),
            ModeratorPanelEventListSortOrder.Duration => events.OrderBy(q => q.Duration).ToList(),
            ModeratorPanelEventListSortOrder.NameDesc => events.OrderByDescending(c => c.Name).ToList(),
            ModeratorPanelEventListSortOrder.Name => events.OrderBy(c => c.Name).ToList(),
            _ => events.OrderBy(c => c.Name).ToList()
        };

        var paginatedEventsDto = sortedEvents
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToList();

        var paginatedList = new PaginatedList<EventForEditListDto>(paginatedEventsDto,
            totalNumber,
            page,
            perPage);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.GetEventsForEditList())
            .Returns(eventsQuery);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.EventsForEditListCountAsync(searchString, _cancellationToken))
            .ReturnsAsync(totalNumber);

        // Act
        var result = await _eventService.GetPagedEventsAsync(sortOrder.ToString(), searchString, page, perPage, _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(paginatedList);
    }

    [Test]
    public async Task GetEventWithDetailsById_ReturnsEventForDetailsDto()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var events = fixture.Build<EventForDetailsDto>()
            .CreateMany()
            .ToList();

        var eventsQuery = events.BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.GetEventsWithDetailsById())
            .Returns(eventsQuery);

        // Act
        var result = await _eventService.GetEventWithDetailsByIdAsync(events.First().Id, _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(events.First());
    }

    [Test]
    public async Task GetEventForEventAreaList_GivenValidId_ReturnsEventForDetailsDto()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var events = fixture.Build<EventInfoForEventAreaListDto>()
            .CreateMany()
            .ToList();

        var eventsQuery = events.BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Event.GetEventsForEventAreaList())
            .Returns(eventsQuery);

        // Act
        var result = await _eventService.GetEventForEventAreaListAsync(events.First().Id, _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(events.First());
    }
}
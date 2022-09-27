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
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using TicketManagement.Core.EventManagement.Services;
using TicketManagement.Core.EventManagement.Services.Interfaces;
using TicketManagement.Core.EventManagement.Services.Validation;
using TicketManagement.Core.Public.DTOs.EventSeatDTOs;
using TicketManagement.Core.Public.Enums;
using TicketManagement.Core.Public.Exceptions;
using TicketManagement.Core.Public.Localization;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.Public;
using TicketManagement.UnitTests.Helpers;

namespace TicketManagement.UnitTests.EventManagement.Services;

internal class EventSeatServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new ();
    private readonly Mock<IMapper> _mapperMock = new ();
    private readonly IValidator<EventSeat> _eventSeatValidator = new EventSeatValidator();
    private readonly CancellationToken _cancellationToken = default;
    private IStringLocalizer<SharedResource> _serviceResourcesStringLocalizer;
    private IStringLocalizer<EventSeatService> _stringLocalizer;
    private IEventSeatService _eventSeatService;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _mapperMock
            .Setup(mapper => mapper.Map<EventSeat>(It.IsAny<EventSeatDto>()))
            .Returns((EventSeatDto dto) =>
            {
                var entity = new EventSeat
                {
                    Id = dto.Id,
                    EventAreaId = dto.EventAreaId,
                    Row = dto.Row,
                    Number = dto.Number,
                    State = (int)dto.State,
                };

                return entity;
            });

        _mapperMock
            .Setup(mapper => mapper.Map<EventSeatDto>(It.IsAny<EventSeat>()))
            .Returns((EventSeat entity) =>
            {
                var dto = new EventSeatDto
                {
                    Id = entity.Id,
                    EventAreaId = entity.EventAreaId,
                    Row = entity.Row,
                    Number = entity.Number,
                    State = (SeatState)entity.State,
                };

                return dto;
            });

        _mapperMock
            .Setup(mapper => mapper.Map<EventSeat>(It.IsAny<EventSeatForListDto>()))
            .Returns((EventSeatForListDto entity) =>
            {
                var dto = new EventSeat
                {
                    Id = entity.Id,
                    Row = entity.Row,
                    Number = entity.Number,
                    State = (int)entity.State,
                };

                return dto;
            });

        _mapperMock
            .Setup(mapper => mapper.Map<EventSeatForListDto>(It.IsAny<EventSeat>()))
            .Returns((EventSeat entity) =>
            {
                var dto = new EventSeatForListDto
                {
                    Id = entity.Id,
                    Row = entity.Row,
                    Number = entity.Number,
                    State = (SeatState)entity.State,
                };

                return dto;
            });

        _mapperMock
            .Setup(mapper => mapper.Map<EventSeatDto>(null))
            .Returns(() =>
            {
                EventSeatDto result = null;
                return result;
            });

        var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
        var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
        _serviceResourcesStringLocalizer = new StringLocalizer<SharedResource>(factory);
        _stringLocalizer = new StringLocalizer<EventSeatService>(factory);
    }

    [SetUp]
    public void Setup()
    {
        _eventSeatService = new EventSeatService(_unitOfWorkMock.Object,
            _mapperMock.Object,
            _eventSeatValidator,
            _serviceResourcesStringLocalizer,
            _stringLocalizer);
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWorkMock.Reset();
    }

    [Test]
    public void UpdateEventSeat_GivenValidEventSeat_ExecutesWithoutException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventSeatDto = fixture.Build<EventSeatDto>()
            .Create();

        var eventSeat = _mapperMock.Object.Map<EventSeat>(eventSeatDto);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventSeat.GetByIdAsync(eventSeat.Id, _cancellationToken))
            .ReturnsAsync(eventSeat);

        _unitOfWorkMock
            .Setup(unitOfWork =>
                unitOfWork.EventSeat.IsExistAsync(It.Is<EventSeat>(eventSeat =>
                    eventSeat.Id == eventSeatDto.Id),
                    _cancellationToken))
            .ReturnsAsync(false);

        // Act, Assert
        Assert.DoesNotThrowAsync(async () => await _eventSeatService.UpdateAsync(eventSeatDto, _cancellationToken));
    }

    [Test]
    public void UpdateEventSeat_GivenEventSeatWithExistedParameters_ExecutesWithException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventSeatDto = fixture.Build<EventSeatDto>()
            .Create();

        var eventSeat = _mapperMock.Object.Map<EventSeat>(eventSeatDto);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventSeat.GetByIdAsync(eventSeat.Id, _cancellationToken))
            .ReturnsAsync(eventSeat);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventSeat.IsAnotherExistAsync(It.Is<EventSeat>(v => v.Id == eventSeatDto.Id), _cancellationToken))
            .ReturnsAsync(true);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _eventSeatService.UpdateAsync(eventSeatDto, _cancellationToken));
    }

    [Test]
    public void UpdateEventSeat_EventSeatRepositoryThrowDbException_ExecutesWithException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventSeatDto = fixture.Build<EventSeatDto>()
            .Create();

        var eventSeat = _mapperMock.Object.Map<EventSeat>(eventSeatDto);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventSeat.GetByIdAsync(eventSeat.Id, _cancellationToken))
            .ReturnsAsync(eventSeat);

        _unitOfWorkMock
            .Setup(unitOfWork =>
                unitOfWork.EventSeat.IsExistAsync(It.Is<EventSeat>(eventSeat =>
                        eventSeat.Id == eventSeatDto.Id),
                    _cancellationToken))
            .ReturnsAsync(false);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(_cancellationToken))
            .ThrowsAsync(new Mock<DbException>().Object);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _eventSeatService.UpdateAsync(eventSeatDto, _cancellationToken));
    }

    [Test]
    public void UpdateEventSeat_EventSeatNotFound_ExecutesWithException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventSeatDto = fixture.Build<EventSeatDto>()
            .Create();

        EventSeat eventSeat = null;

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventSeat.GetByIdAsync(eventSeatDto.Id, _cancellationToken))
            .ReturnsAsync(eventSeat);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _eventSeatService.UpdateAsync(eventSeatDto, _cancellationToken));
    }

    [Test]
    public void DeleteEventSeat_ExecutesWithoutException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventSeat = fixture.Create<EventSeat>();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventSeat.GetByIdAsync(eventSeat.Id, _cancellationToken))
            .ReturnsAsync(eventSeat);

        // Act, Assert
        Assert.DoesNotThrowAsync(async () => await _eventSeatService.DeleteAsync(eventSeat.Id, _cancellationToken));
    }

    [Test]
    public void DeleteEventSeat_EventSeatRepositoryThrowDbException_ExecutesWithException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventSeat = fixture.Create<EventSeat>();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventSeat.GetByIdAsync(eventSeat.Id, _cancellationToken))
            .ReturnsAsync(eventSeat);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(_cancellationToken))
            .ThrowsAsync(new Mock<DbException>().Object);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _eventSeatService.DeleteAsync(eventSeat.Id, _cancellationToken));
    }

    [Test]
    public void DeleteEventSeat_EventSeatNotFound_ExecutesWithException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventSeatId = fixture.Create<int>();

        EventSeat eventSeat = null;

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventSeat.GetByIdAsync(eventSeatId, _cancellationToken))
            .ReturnsAsync(eventSeat);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _eventSeatService.DeleteAsync(eventSeatId, _cancellationToken));
    }

    [Test]
    public async Task GetById_GivenValidId_ReturnsEventSeatDto()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventSeatDto = fixture.Build<EventSeatDto>()
            .Create();

        var eventSeat = _mapperMock.Object.Map<EventSeat>(eventSeatDto);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventSeat.GetByIdAsync(eventSeat.Id, _cancellationToken))
            .ReturnsAsync(eventSeat);

        // Act
        var result = await _eventSeatService.GetByIdAsync(eventSeat.Id, _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(eventSeatDto);
    }

    [Test]
    public async Task GetById_GivenInvalidId_ReturnsNull()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventSeatId = fixture.Create<int>();

        EventSeat eventSeat = null;

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventSeat.GetByIdAsync(eventSeatId, _cancellationToken))
            .ReturnsAsync(eventSeat);

        // Act
        var result = await _eventSeatService.GetByIdAsync(eventSeatId, _cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GetEventSeatsByEventAreaId_GivenValidId_ReturnsIEnumerableEventSeatDto()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventAreaId = fixture.Create<int>();

        var eventSeatsForListDto = fixture.Build<EventSeatForListDto>()
            .CreateMany()
            .ToList();

        var eventSeats = eventSeatsForListDto
            .Select(eventSeatDto =>
            {
                var eventSeat = _mapperMock.Object.Map<EventSeat>(eventSeatDto);
                eventSeat.EventAreaId = eventAreaId;
                return eventSeat;
            })
            .ToList();

        var eventSeatsQuery = eventSeats.BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventSeat.FindBy(It.IsAny<Expression<Func<EventSeat, bool>>>()))
            .Returns(eventSeatsQuery);

        // Act
        var result = await _eventSeatService.GetAllEventSeatsByEventAreaIdAsync(eventAreaId, _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(eventSeatsForListDto);
    }

    [Test]
    public async Task GetEventSeatsByEventAreaId_GivenInvalidId_ReturnsEmptyIEnumerable()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventAreaId = fixture.Create<int>();

        var eventSeatsForListDto = Enumerable.Empty<EventSeatForListDto>();

        var eventSeats = eventSeatsForListDto
            .Select(eventSeatDto => _mapperMock.Object.Map<EventSeat>(eventSeatDto))
            .ToList();

        var eventSeatsQuery = eventSeats.BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventSeat.FindBy(It.IsAny<Expression<Func<EventSeat, bool>>>()))
            .Returns(eventSeatsQuery);

        // Act
        var result = await _eventSeatService.GetAllEventSeatsByEventAreaIdAsync(eventAreaId, _cancellationToken);

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetPagedEventSeatsByEventId_GivenValidId_ReturnsIEnumerableEventSeatDto()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var eventAreaId = fixture.Create<int>();

        var totalNumber = fixture.Create<int>();

        var perPage = 5;

        var page = 1;

        var eventSeatsDto = fixture.Build<EventSeatDto>()
            .With(eventSeatForListDto => eventSeatForListDto.EventAreaId, eventAreaId)
            .CreateMany(totalNumber)
            .ToList();

        var eventSeats = eventSeatsDto
            .Select(eventSeatDto => _mapperMock.Object.Map<EventSeat>(eventSeatDto))
            .ToList();

        var paginatedEventSeatsDto = eventSeatsDto
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToList();

        var eventSeatsQuery = eventSeats.BuildMock();

        var paginatedList = new PaginatedList<EventSeatDto>(paginatedEventSeatsDto,
            totalNumber,
            page,
            perPage);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.EventSeat.GetEventSeatsByEventAreaId(eventAreaId))
            .Returns(eventSeatsQuery);

        // Act
        var result = await _eventSeatService.GetPagedEventSeatsByEventAreaIdAsync(eventAreaId, page, perPage, _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(paginatedList);
    }
}
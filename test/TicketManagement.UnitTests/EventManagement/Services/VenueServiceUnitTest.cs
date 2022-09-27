using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using TicketManagement.Core.EventManagement.Services;
using TicketManagement.Core.Public.DTOs.VenueDTOs;
using TicketManagement.Core.Public.Exceptions;
using TicketManagement.Core.Public.Localization;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.Public;
using TicketManagement.UnitTests.Helpers;

namespace TicketManagement.UnitTests.EventManagement.Services;

internal class VenueServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new ();
    private readonly Mock<IMapper> _mapperMock = new ();
    private IStringLocalizer<SharedResource> _serviceResourcesStringLocalizer;
    private VenueService _venueService;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _mapperMock
            .Setup(mapper => mapper.Map<Venue>(It.IsAny<VenueDto>()))
            .Returns((VenueDto dto) =>
            {
                var entity = new Venue
                {
                    Id = dto.Id,
                    Description = dto.Description,
                    Address = dto.Address,
                    Phone = dto.Phone,
                    TimeZoneId = dto.TimeZoneId,
                };

                return entity;
            });

        _mapperMock
            .Setup(mapper => mapper.Map<VenueDto>(It.IsAny<Venue>()))
            .Returns((Venue entity) =>
            {
                var dto = new VenueDto
                {
                    Id = entity.Id,
                    Description = entity.Description,
                    Address = entity.Address,
                    Phone = entity.Phone,
                    TimeZoneId = entity.TimeZoneId,
                };

                return dto;
            });

        _mapperMock
            .Setup(mapper => mapper.Map<VenueDto>(null))
            .Returns(() =>
            {
                VenueDto result = null;
                return result;
            });

        var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
        var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
        _serviceResourcesStringLocalizer = new StringLocalizer<SharedResource>(factory);
    }

    [SetUp]
    public void Setup()
    {
        _venueService = new VenueService(_unitOfWorkMock.Object, _mapperMock.Object, _serviceResourcesStringLocalizer);
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWorkMock.Reset();
    }

    [Test]
    public async Task GetByIdAsync_GivenValidId_ReturnsVenueDto()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var venueDto = fixture.Build<VenueDto>()
            .With(dto => dto.Phone, fixture.Create<string>()[..30])
            .Create();

        var venue = _mapperMock.Object.Map<Venue>(venueDto);

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Venue.GetByIdAsync(venue.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(venue);

        // Act
        var result = await _venueService.GetByIdAsync(venue.Id);

        // Assert
        result.Should().BeEquivalentTo(venueDto);
    }

    [Test]
    public void GetByIdAsync_GivenInvalidId_TrowException()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var venueId = fixture.Create<int>();

        Venue venue = null;

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Venue.GetByIdAsync(venueId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(venue);

        // Act, Assert
        Assert.ThrowsAsync<ServiceException>(async () => await _venueService.GetByIdAsync(venueId));
    }

    [Test]
    public async Task GetAllVenuesAsync_ReturnsIEnumerableVenueDto()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var venues = fixture.Build<Venue>()
            .CreateMany()
            .BuildMock();

        var venuesDto = venues
            .Select(venue => _mapperMock.Object.Map<VenueDto>(venue))
            .ToList();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Venue.FindAll())
            .Returns(venues);

        // Act
        var result = await _venueService.GetAllVenuesAsync();

        // Assert
        result.Should().BeEquivalentTo(venuesDto);
    }

    [Test]
    public async Task GetAreasByLayoutIdAsync_GivenInvalidId_ReturnsEmpty()
    {
        // Arrange
        var venues = Enumerable.Empty<Venue>()
            .BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Venue.FindAll())
            .Returns(venues);

        // Act
        var result = await _venueService.GetAllVenuesAsync();

        // Assert
        result.Should().BeEmpty();
    }
}
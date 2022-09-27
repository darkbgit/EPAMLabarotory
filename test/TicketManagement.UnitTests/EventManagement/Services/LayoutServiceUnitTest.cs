using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using TicketManagement.Core.EventManagement.Services;
using TicketManagement.Core.Public.DTOs.LayoutDTOs;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.Public;
using TicketManagement.UnitTests.Helpers;

namespace TicketManagement.UnitTests.EventManagement.Services;

internal class LayoutServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new ();
    private readonly Mock<IMapper> _mapperMock = new ();
    private LayoutService _layoutService;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _mapperMock
            .Setup(mapper => mapper.Map<Layout>(It.IsAny<LayoutDto>()))
            .Returns((LayoutDto dto) =>
            {
                var entity = new Layout
                {
                    Id = dto.Id,
                    Description = dto.Description,
                    VenueId = dto.VenueId,
                };

                return entity;
            });

        _mapperMock
            .Setup(mapper => mapper.Map<LayoutDto>(It.IsAny<Layout>()))
            .Returns((Layout entity) =>
            {
                var dto = new LayoutDto
                {
                    Id = entity.Id,
                    Description = entity.Description,
                    VenueId = entity.VenueId,
                };

                return dto;
            });

        _mapperMock
            .Setup(mapper => mapper.Map<LayoutDto>(null))
            .Returns(() =>
            {
                LayoutDto result = null;
                return result;
            });
    }

    [SetUp]
    public void Setup()
    {
        _layoutService = new LayoutService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWorkMock.Reset();
    }

    [Test]
    public async Task GetLayoutsByVenueIdAsync_GivenValidId_RuturnsIEnumerableLayoutDto()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var venueId = fixture.Create<int>();

        var layouts = fixture.Build<Layout>()
            .With(layout => layout.VenueId, venueId)
            .CreateMany()
            .BuildMock();

        var layoutsDto = layouts
            .Select(layout => _mapperMock.Object.Map<LayoutDto>(layout))
            .ToList();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Layout.GetLayoutsByVenueId(venueId))
            .Returns(layouts);

        // Act
        var result = await _layoutService.GetLayoutsByVenueIdAsync(venueId);

        // Assert
        result.Should().BeEquivalentTo(layoutsDto);
    }

    [Test]
    public async Task GetLayoutsByVenueIdAsync_GivenInvalidId_RuturnsEmpty()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var venueId = fixture.Create<int>();

        var layouts = Enumerable.Empty<Layout>()
            .BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Layout.GetLayoutsByVenueId(venueId))
            .Returns(layouts);

        // Act
        var result = await _layoutService.GetLayoutsByVenueIdAsync(venueId);

        // Assert
        result.Should().BeEmpty();
    }
}
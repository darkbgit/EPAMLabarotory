using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using TicketManagement.Core.EventManagement.Services;
using TicketManagement.Core.EventManagement.Services.Interfaces;
using TicketManagement.Core.Public.DTOs.AreaDTOs;
using TicketManagement.DataAccess.EF.Core;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.Public;
using TicketManagement.UnitTests.Helpers;

namespace TicketManagement.UnitTests.EventManagement.Services;

internal class AreaServiceUnitTest
{
    private readonly Mock<IMapper> _mapperMock = new ();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new ();
    private IAreaService _areaService;

    private DbContextOptions<TicketManagementContext> Options { get; } = new DbContextOptionsBuilder<TicketManagementContext>().Options;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _mapperMock
            .Setup(mapper => mapper.Map<Area>(It.IsAny<AreaDto>()))
            .Returns((AreaDto dto) =>
            {
                var entity = new Area
                {
                    Id = dto.Id,
                    Description = dto.Description,
                    LayoutId = dto.LayoutId,
                    CoordX = dto.CoordX,
                    CoordY = dto.CoordY,
                };

                return entity;
            });

        _mapperMock
            .Setup(mapper => mapper.Map<AreaDto>(It.IsAny<Area>()))
            .Returns((Area entity) =>
            {
                var dto = new AreaDto
                {
                    Id = entity.Id,
                    Description = entity.Description,
                    LayoutId = entity.LayoutId,
                    CoordX = entity.CoordX,
                    CoordY = entity.CoordY,
                };

                return dto;
            });

        _mapperMock
            .Setup(mapper => mapper.Map<AreaDto>(null))
            .Returns(() =>
            {
                AreaDto result = null;
                return result;
            });
    }

    [SetUp]
    public void Setup()
    {
        _areaService = new AreaService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWorkMock.Reset();
    }

    [Test]
    public async Task GetAreasByLayoutIdAsync_GivenValidId_ReturnsIEnumerableAreaDto()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var layoutId = fixture.Create<int>();

        var areasDto = fixture.Build<AreaDto>()
            .With(areaDto => areaDto.LayoutId, layoutId)
            .CreateMany()
            .ToList();

        var areas = areasDto
            .Select(i => _mapperMock.Object.Map<Area>(i))
            .BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Area.GetAreasByLayoutId(layoutId))
            .Returns(areas);

        // Act
        var result = await _areaService.GetAreasByLayoutIdAsync(layoutId);

        // Assert
        result.Should().BeEquivalentTo(areasDto);
    }

    [Test]
    public async Task GetAreasByLayoutIdAsync_GivenInvalidId_ReturnsEmpty()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var layoutId = fixture.Create<int>();

        var areas = Enumerable.Empty<Area>()
            .BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Area.GetAreasByLayoutId(layoutId))
            .Returns(areas);

        // Act
        var result = await _areaService.GetAreasByLayoutIdAsync(layoutId);

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetAreasWithSeatsNumberByLayoutIdAsync_GivenValidId_ReturnsIEnumerableAreaDto()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var layoutId = fixture.Create<int>();

        var areasDto = fixture.Build<AreaWithSeatsNumberDto>()
            .With(areaWithSeatsNumberDto => areaWithSeatsNumberDto.LayoutId, layoutId)
            .CreateMany()
            .BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Area.GetAreasWithSeatsNumberByLayoutId(layoutId))
            .Returns(areasDto);

        // Act
        var result = await _areaService.GetAreasWithSeatsNumberByLayoutIdAsync(layoutId);

        // Assert
        result.Should().BeEquivalentTo(areasDto);
    }

    [Test]
    public async Task GetAreasWithSeatsNumberByLayoutIdAsync_GivenInvalidId_ReturnsEmpty()
    {
        // Arrange
        var fixture = FixtureFactory.GetFixture();

        var layoutId = fixture.Create<int>();

        var areas = Enumerable.Empty<AreaWithSeatsNumberDto>()
            .BuildMock();

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Area.GetAreasWithSeatsNumberByLayoutId(layoutId))
            .Returns(areas);

        // Act
        var result = await _areaService.GetAreasWithSeatsNumberByLayoutIdAsync(layoutId);

        // Assert
        result.Should().BeEmpty();
    }
}
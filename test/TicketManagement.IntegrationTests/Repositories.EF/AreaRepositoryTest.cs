using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TicketManagement.Core.Public.DTOs.AreaDTOs;
using TicketManagement.DataAccess.EF.Implementation.Repositories;
using TicketManagement.DataAccess.Public.Repositories;
using TicketManagement.IntegrationTests.Helpers;

namespace TicketManagement.IntegrationTests.Repositories.EF;

internal class AreaRepositoryTest : BaseTest
{
    [Test]
    public async Task GetAreasByLayoutId_GivenValidLayoutId_ReturnsArea()
    {
        // Arrange
        await using var context = GetContextWithData();
        IAreaRepository areaRepository = new AreaRepository(context);

        var layoutId = TestData.Areas.First().LayoutId;

        var testAreas = TestData.Areas
            .Where(a => a.LayoutId == layoutId);

        // Act
        var areas = areaRepository.GetAreasByLayoutId(layoutId);

        var result = await areas.ToListAsync();

        // Assert
        result.Should().NotBeEmpty();

        result.Should().BeEquivalentTo(testAreas);
    }

    [Test]
    public async Task GetAreasByLayoutId_GivenInvalidLayoutId_ReturnsEmpty()
    {
        // Arrange
        await using var context = GetContextWithData();
        IAreaRepository areaRepository = new AreaRepository(context);

        var layoutId = TestData.Areas.Max(area => area.LayoutId);
        layoutId++;

        // Act
        var areas = areaRepository.GetAreasByLayoutId(layoutId);

        var result = await areas.ToListAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetAreasWithSeatsNumberByLayoutId_GivenValidLayoutId_ReturnsEmpty()
    {
        // Arrange
        await using var context = GetContextWithData();
        IAreaRepository areaRepository = new AreaRepository(context);

        var layoutId = TestData.Areas.First().LayoutId;

        var testData = TestData.Areas
            .Where(area => area.LayoutId == layoutId)
            .GroupJoin(TestData.Seats, area => area.Id, seat => seat.AreaId,
                (area, seats) => new { area, seats })
            .SelectMany(@areaSeats => @areaSeats.seats.DefaultIfEmpty(),
                (@areaSeats, seat) => new { @areaSeats, seat })
            .GroupBy(@areaSeatsSeat => new
            {
                @areaSeatsSeat.@areaSeats.area.Id,
                @areaSeatsSeat.@areaSeats.area.Description,
                @areaSeatsSeat.@areaSeats.area.CoordX,
                @areaSeatsSeat.@areaSeats.area.CoordY,
            }, @areaSeatsSeat => @areaSeatsSeat.seat)
            .Select(g => new AreaWithSeatsNumberDto
            {
                Id = g.Key.Id,
                Description = g.Key.Description,
                CoordX = g.Key.CoordX,
                CoordY = g.Key.CoordY,
                SeatsNumber = g.Count(s => s != null),
            })
            .ToList();

        // Act
        var areas = areaRepository.GetAreasWithSeatsNumberByLayoutId(layoutId);

        var result = await areas.ToListAsync();

        // Assert
        result.Should().BeEquivalentTo(testData);
    }
}
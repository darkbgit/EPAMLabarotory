using TicketManagement.Core.Public.DTOs.AreaDTOs;
using TicketManagement.DataAccess.EF.Core;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.EF.Implementation.Repositories.Base;
using TicketManagement.DataAccess.Public.Repositories;

namespace TicketManagement.DataAccess.EF.Implementation.Repositories;

internal sealed class AreaRepository : Repository<Area>, IAreaRepository
{
    public AreaRepository(TicketManagementContext context)
        : base(context)
    {
    }

    public IQueryable<Area> GetAreasByLayoutId(int layoutId)
    {
        var query = Table.Where(area => area.LayoutId == layoutId);

        return query;
    }

    public IQueryable<AreaWithSeatsNumberDto> GetAreasWithSeatsNumberByLayoutId(int layoutId)
    {
        var query = Table
            .Where(area => area.LayoutId == layoutId)
            .GroupJoin(Context.Seat, area => area.Id, seat => seat.AreaId,
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
            });

        return query;
    }
}
using TicketManagement.DataAccess.EF.Core;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.EF.Implementation.Repositories.Base;
using TicketManagement.DataAccess.Public.Repositories;

namespace TicketManagement.DataAccess.EF.Implementation.Repositories;

internal sealed class SeatRepository : Repository<Seat>, ISeatRepository
{
    public SeatRepository(TicketManagementContext context)
        : base(context)
    {
    }

    public IQueryable<Seat> GetSeatsByAreaId(int areaId)
    {
        var result = Table.Where(seat => seat.AreaId == areaId);

        return result;
    }
}
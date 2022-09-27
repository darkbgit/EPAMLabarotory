using TicketManagement.DataAccess.EF.Core;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.EF.Implementation.Repositories.Base;
using TicketManagement.DataAccess.Public.Repositories;

namespace TicketManagement.DataAccess.EF.Implementation.Repositories;

internal sealed class LayoutRepository : Repository<Layout>, ILayoutRepository
{
    public LayoutRepository(TicketManagementContext context)
        : base(context)
    {
    }

    public IQueryable<Layout> GetLayoutsByVenueId(int venueId)
    {
        var query = Table.Where(layout => layout.VenueId == venueId);

        return query;
    }
}
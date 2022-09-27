using TicketManagement.DataAccess.EF.Core;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.EF.Implementation.Repositories.Base;
using TicketManagement.DataAccess.Public.Repositories;

namespace TicketManagement.DataAccess.EF.Implementation.Repositories;

internal sealed class VenueRepository : Repository<Venue>, IVenueRepository
{
    public VenueRepository(TicketManagementContext context)
        : base(context)
    {
    }

    public void CreateVenue(Venue venue)
    {
        Add(venue);
    }

    public void UpdateVenue(Venue venue)
    {
        Update(venue);
    }

    public void DeleteVenue(Venue venue)
    {
        Remove(venue);
    }
}
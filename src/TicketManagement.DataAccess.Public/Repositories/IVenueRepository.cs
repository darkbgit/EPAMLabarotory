using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.DataAccess.Public.Repositories
{
    public interface IVenueRepository : IRepository<Venue>
    {
        void CreateVenue(Venue venue);
        void UpdateVenue(Venue venue);
        void DeleteVenue(Venue venue);
    }
}
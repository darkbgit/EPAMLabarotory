using System.Linq;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.DataAccess.Public.Repositories
{
    public interface ILayoutRepository : IRepository<Layout>
    {
        IQueryable<Layout> GetLayoutsByVenueId(int venueId);
    }
}

using System.Linq;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.DataAccess.Public.Repositories
{
    public interface ISeatRepository : IRepository<Seat>
    {
        IQueryable<Seat> GetSeatsByAreaId(int areaId);
    }
}
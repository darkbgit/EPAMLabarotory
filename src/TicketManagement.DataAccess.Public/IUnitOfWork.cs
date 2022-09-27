using System.Threading;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Public.Repositories;

namespace TicketManagement.DataAccess.Public
{
    public interface IUnitOfWork
    {
        IEventRepository Event { get; }
        IVenueRepository Venue { get; }
        ILayoutRepository Layout { get; }
        IAreaRepository Area { get; }
        ISeatRepository Seat { get; }
        IEventAreaRepository EventArea { get; }
        IEventSeatRepository EventSeat { get; }
        IOrderRepository Order { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.DataAccess.Public.Repositories
{
    public interface IEventSeatRepository : IRepository<EventSeat>
    {
        IQueryable<EventSeat> GetEventSeatsByEventAreaId(int eventAreaId);

        Task UpdateStateAsync(int eventSeatId, int newState, CancellationToken cancellationToken);

        Task<bool> IsExistAsync(EventSeat entity, CancellationToken cancellationToken);

        Task<bool> IsAnotherExistAsync(EventSeat entity, CancellationToken cancellationToken);

        Task<bool> IsExistWithStateAsync(int eventSeatId, int seatState, CancellationToken cancellationToken);

        void AddEventSeatsRange(IEnumerable<EventSeat> eventSeats);

        void CreateEventSeat(EventSeat entity);
        void UpdateEventSeat(EventSeat entity);
        void DeleteEventSeat(EventSeat entity);
    }
}
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketManagement.Core.Public.DTOs.EventAreaDTOs;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.DataAccess.Public.Repositories
{
    public interface IEventAreaRepository : IRepository<EventArea>
    {
        Task<bool> IsExistAsync(EventArea entity, CancellationToken cancellationToken);
        Task<bool> IsAnotherExistAsync(EventArea entity, CancellationToken cancellationToken);
        IQueryable<EventArea> GetEventAreasByEventId(int eventId);
        IQueryable<EventAreaWithSeatsNumberDto> GetEventAreasWithSeatsCountByEventId(int eventId);
        IQueryable<EventAreaWithSeatsAndFreeSeatsCountDto> GetEventAreasWithSeatsAndFreeSeatsCountByEventId(int eventId);
        void CreateEventArea(EventArea entity);
        void UpdateEventArea(EventArea entity);
        void DeleteEventArea(EventArea entity);
    }
}
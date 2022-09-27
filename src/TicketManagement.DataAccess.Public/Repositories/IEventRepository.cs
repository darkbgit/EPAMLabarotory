using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketManagement.Core.Public.DTOs.EventDTOs;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.DataAccess.Public.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        IQueryable<EventForListDto> GetEventsForMainPage();
        IQueryable<EventForEditListDto> GetEventsForEditList();
        IQueryable<EventWithVenueIdDto> GetEventsWithVenueIdById();
        IQueryable<EventForDetailsDto> GetEventsWithDetailsById();
        IQueryable<EventInfoForEventAreaListDto> GetEventsForEventAreaList();
        IQueryable<EventWithLayoutsDto> GetEventsWithLayoutsAndVenueTimeZoneById();

        Task<int> EventsForEditListCountAsync(string searchString, CancellationToken cancellationToken);
        Task<int> EventsForManePageCountAsync(CancellationToken cancellationToken);
        Task<bool> IsExistAsync(Event entity, CancellationToken cancellationToken);
        Task<bool> IsExistForUpdateAsync(Event entity, CancellationToken cancellationToken);

        int CreateEvent(Event entity);
        int UpdateEvent(Event entity);
        int DeleteEvent(int eventId);
    }
}
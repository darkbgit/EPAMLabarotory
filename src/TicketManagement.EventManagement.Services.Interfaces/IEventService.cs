using TicketManagement.Core.Public.DTOs.EventDTOs;
using TicketManagement.Core.Public.Models.Pagination;

namespace TicketManagement.Core.EventManagement.Services.Interfaces
{
    public interface IEventService
    {
        Task<PaginatedList<EventForListDto>> GetPagedUpcomingEventsAsync(int page, int eventsPerPage, CancellationToken cancellationToken = default);

        Task<PaginatedList<EventForEditListDto>> GetPagedEventsAsync(
            string sortOrder,
            string searchString,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<EventForDetailsDto?> GetEventWithDetailsByIdAsync(int eventId,
            CancellationToken cancellationToken = default);

        Task<EventInfoForEventAreaListDto?> GetEventForEventAreaListAsync(int eventId, CancellationToken cancellationToken = default);

        Task<EventWithVenueIdDto?> GetEventWithVenueIdByIdAsync(int eventId, CancellationToken cancellationToken = default);

        Task<EventWithLayoutsDto?> GetEventWithLayoutsByIdAsync(int eventId, CancellationToken cancellationToken = default);

        Task<EventDto?> GetByIdAsync(int eventId, CancellationToken cancellationToken = default);

        Task<int> CreateAsync(EventForCreateDto eventForCreateDto, CancellationToken cancellationToken = default);

        Task<int> CreateAsync(ThirdPartyEventDto thirdPartyEventDto, CancellationToken cancellationToken = default);

        Task UpdateAsync(EventForUpdateDto eventDto, CancellationToken cancellationToken = default);

        Task DeleteAsync(int eventId, CancellationToken cancellationToken = default);
    }
}
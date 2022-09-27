using Refit;
using TicketManagement.Core.Public.DTOs.EventDTOs;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.Core.Public.Requests;

namespace TicketManagement.MVC.Clients.EventManagement
{
    public interface IEventsClient
    {
        [Get("/events/edit-list")]
        Task<PaginatedList<EventForEditListDto>> GetPaginatedEventsForEditList(PaginationSearchSortRequest query);

        [Get("/events/main-page")]
        Task<PaginatedList<EventForListDto>> GetPaginatedEventsForMainPage(PaginationRequest query);

        [Get("/events/with-layouts/{id}")]
        Task<EventWithLayoutsDto?> GetEventWithLayouts(int id);

        [Get("/events/with-layout-name/{id}")]
        Task<EventInfoForEventAreaListDto?> GetEventWithLayoutName(int id);

        [Get("/events/with-details/{id}")]
        Task<EventForDetailsDto?> GetEventWithDetails(int id);

        [Get("/events/{id}")]
        Task<EventDto?> GetEventById(int id);

        [Post("/events")]
        Task<EventDto> CreateEvent([Body] EventForCreateDto dto);

        [Post("/events/third-party-editor")]
        Task<EventDto> CreateEventFromThirdPartyEditor([Body] ThirdPartyEventDto dto);

        [Put("/events/{id}")]
        Task UpdateEvent(int id, [Body] EventForUpdateDto dto);

        [Delete("/events/{id}")]
        Task DeleteEvent(int id);
    }
}
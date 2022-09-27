using Refit;
using TicketManagement.Core.Public.DTOs.EventAreaDTOs;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.Core.Public.Requests;

namespace TicketManagement.MVC.Clients.EventManagement
{
    public interface IEventAreasClient
    {
        [Get("/events/{id}/event-areas/paginated-with-seat")]
        Task<PaginatedList<EventAreaWithSeatsNumberDto>> GetPaginatedEventAreasForEditList(int id, PaginationRequest query);

        [Get("/events/{id}/event-areas/with-seats")]
        Task<List<EventAreaWithSeatsAndFreeSeatsCountDto>> GetAllEventAreasByEventId(int id);

        [Get("/event-areas/{id}")]
        Task<EventAreaDto> GetEventAreaById(int id);

        [Put("/event-areas/{id}")]
        Task UpdateEventArea(int id, [Body] EventAreaDto eventArea);

        [Delete("/event-areas/{id}")]
        Task DeleteEventArea(int id);
    }
}

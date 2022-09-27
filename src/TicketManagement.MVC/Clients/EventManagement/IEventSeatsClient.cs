using Refit;
using TicketManagement.Core.Public.DTOs.EventSeatDTOs;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.Core.Public.Requests;

namespace TicketManagement.MVC.Clients.EventManagement
{
    public interface IEventSeatsClient
    {
        [Get("/event-areas/{id}/event-seats/paginated")]
        Task<PaginatedList<EventSeatDto>> GetPaginatedEventSeatsForEditList(int id, PaginationRequest query);

        [Get("/event-areas/{id}/event-seats")]
        Task<List<EventSeatForListDto>> GetAllEventSeatsByEventAreaId(int id);

        [Get("/event-seats/{id}")]
        Task<EventSeatDto> GetEventSeatById(int id);

        [Put("/event-areas/{id}")]
        Task UpdateEventSeat(int id, [Body] EventSeatDto eventArea);

        [Delete("/event-areas/{id}")]
        Task DeleteEventSeat(int id);
    }
}
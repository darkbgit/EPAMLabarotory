using TicketManagement.Core.Public.DTOs.EventSeatDTOs;
using TicketManagement.Core.Public.Models.Pagination;

namespace TicketManagement.Core.EventManagement.Services.Interfaces
{
    public interface IEventSeatService
    {
        Task<IEnumerable<EventSeatForListDto>> GetAllEventSeatsByEventAreaIdAsync(int eventAreaId, CancellationToken cancellationToken = default);

        Task<PaginatedList<EventSeatDto>> GetPagedEventSeatsByEventAreaIdAsync(int eventAreaId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        Task<EventSeatDto?> GetByIdAsync(int eventSeatId, CancellationToken cancellationToken = default);

        Task UpdateAsync(EventSeatDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int eventSeatId, CancellationToken cancellationToken = default);
    }
}
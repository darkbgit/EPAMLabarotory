using TicketManagement.Core.Public.DTOs.EventAreaDTOs;
using TicketManagement.Core.Public.Models.Pagination;

namespace TicketManagement.Core.EventManagement.Services.Interfaces;

public interface IEventAreaService
{
    Task<IEnumerable<EventAreaWithSeatsAndFreeSeatsCountDto>> GetEventAreasWithSeatsInfoByEventIdAsync(int eventId, CancellationToken cancellationToken = default);

    Task<PaginatedList<EventAreaWithSeatsNumberDto>> GetPagedEventAreasByEventIdAsync(int eventId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    Task<int> GetEventIdByEventAreaIdAsync(int eventAreaId, CancellationToken cancellationToken = default);

    Task<EventAreaDto?> GetByIdAsync(int eventAreaId, CancellationToken cancellationToken = default);

    Task UpdateAsync(EventAreaDto dto, CancellationToken cancellationToken = default);

    Task DeleteAsync(int eventAreaId, CancellationToken cancellationToken = default);
}
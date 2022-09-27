using TicketManagement.Core.Public.DTOs.AreaDTOs;

namespace TicketManagement.Core.EventManagement.Services.Interfaces
{
    public interface IAreaService
    {
        Task<IEnumerable<AreaDto>> GetAreasByLayoutIdAsync(int layoutId, CancellationToken cancellationToken = default);
        Task<IEnumerable<AreaWithSeatsNumberDto>> GetAreasWithSeatsNumberByLayoutIdAsync(int layoutId, CancellationToken cancellationToken = default);
    }
}
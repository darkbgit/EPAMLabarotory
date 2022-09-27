using TicketManagement.Core.Public.DTOs.LayoutDTOs;

namespace TicketManagement.Core.EventManagement.Services.Interfaces
{
    public interface ILayoutService
    {
        Task<IEnumerable<LayoutDto>> GetLayoutsByVenueIdAsync(int venueId, CancellationToken cancellationToken = default);
    }
}

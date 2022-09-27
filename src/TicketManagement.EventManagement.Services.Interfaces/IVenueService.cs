using TicketManagement.Core.Public.DTOs.VenueDTOs;

namespace TicketManagement.Core.EventManagement.Services.Interfaces;

public interface IVenueService
{
    Task<IEnumerable<VenueDto>> GetAllVenuesAsync(CancellationToken cancellationToken = default);
}
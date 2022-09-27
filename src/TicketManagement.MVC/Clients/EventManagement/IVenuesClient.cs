using Refit;
using TicketManagement.Core.Public.DTOs.VenueDTOs;

namespace TicketManagement.MVC.Clients.EventManagement
{
    public interface IVenuesClient
    {
        [Get("/venues")]
        Task<List<VenueDto>> GetAllVenues();
    }
}
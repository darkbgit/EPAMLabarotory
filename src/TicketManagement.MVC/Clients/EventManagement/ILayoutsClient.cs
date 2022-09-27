using Refit;
using TicketManagement.Core.Public.DTOs.LayoutDTOs;

namespace TicketManagement.MVC.Clients.EventManagement
{
    public interface ILayoutsClient
    {
        [Get("/venues/{id}/layouts")]
        Task<List<LayoutDto>> GetLayoutsByVenueId(int id);
    }
}
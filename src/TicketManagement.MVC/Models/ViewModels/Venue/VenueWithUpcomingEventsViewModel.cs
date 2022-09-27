using TicketManagement.Core.Public.DTOs.EventDTOs;
using TicketManagement.Core.Public.DTOs.VenueDTOs;

namespace TicketManagement.MVC.Models.ViewModels.Venue
{
    public class VenueWithUpcomingEventsViewModel : VenueDto
    {
        public IEnumerable<EventForListDto> UpcomingEvents { get; set; }
    }
}
using TicketManagement.Core.Public.DTOs.EventAreaDTOs;

namespace TicketManagement.MVC.Models.ViewModels.EventArea
{
    public class EventAreaListWithInfoViewModel
    {
        public IEnumerable<EventAreaWithSeatsAndFreeSeatsCountDto> EventAreas { get; set; }
        public string EventName { get; set; }
        public string LayoutName { get; set; }
        public DateTime StartDate { get; set; }
    }
}
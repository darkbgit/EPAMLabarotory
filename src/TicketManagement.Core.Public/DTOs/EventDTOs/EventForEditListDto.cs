using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Core.Public.DTOs.EventDTOs
{
    public class EventForEditListDto
    {
        public int Id { get; set; }

        [Display(Name = "Venue description")]
        public string VenueDescription { get; set; }

        [Display(Name = "Layout description")]
        public string LayoutDescription { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Duration")]
        public TimeSpan Duration { get; set; }

        [Display(Name = "Number of event areas")]
        public int EventAreasCount { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Core.Public.DTOs.EventAreaDTOs
{
    public class EventAreaWithSeatsNumberDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Coordinate X")]
        public int CoordX { get; set; }

        [Display(Name = "Coordinate Y")]
        public int CoordY { get; set; }

        [Display(Name = "Price")]
        public decimal? Price { get; set; }

        [Display(Name = "Number of seats")]
        public int TotalSeats { get; set; }
    }
}
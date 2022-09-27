using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Core.Public.DTOs.EventAreaDTOs
{
    public class EventAreaDto
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "CoordX")]
        [Range(0, int.MaxValue, ErrorMessage = "Coordinate X must be positive number.")]
        public int CoordX { get; set; }

        [Display(Name = "CoordY")]
        [Range(0, int.MaxValue)]
        public int CoordY { get; set; }

        [Display(Name = "Price")]
        public decimal? Price { get; set; }
    }
}
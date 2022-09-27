using System.ComponentModel.DataAnnotations;
using TicketManagement.Core.Public.Enums;

namespace TicketManagement.Core.Public.DTOs.EventSeatDTOs
{
    public class EventSeatDto
    {
        public int Id { get; set; }

        public int EventAreaId { get; set; }

        [Display(Name = "Row")]
        public int Row { get; set; }

        [Display(Name = "Number")]
        public int Number { get; set; }

        [Display(Name = "State")]
        public SeatState State { get; set; }
    }
}
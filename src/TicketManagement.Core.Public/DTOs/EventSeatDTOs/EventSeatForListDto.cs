using TicketManagement.Core.Public.Enums;

namespace TicketManagement.Core.Public.DTOs.EventSeatDTOs
{
    public class EventSeatForListDto
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public int Number { get; set; }
        public SeatState State { get; set; }
    }
}
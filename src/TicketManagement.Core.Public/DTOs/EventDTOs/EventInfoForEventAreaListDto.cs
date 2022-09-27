namespace TicketManagement.Core.Public.DTOs.EventDTOs
{
    public class EventInfoForEventAreaListDto
    {
        public int Id { get; set; }
        public string LayoutName { get; set; }
        public string EventName { get; set; }
        public DateTime StartDate { get; set; }
    }
}
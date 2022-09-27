namespace TicketManagement.Core.Public.DTOs.OrderDTOs
{
    public class OrderTicketDto
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public int Number { get; set; }
        public string VenueName { get; set; }
        public string LayoutName { get; set; }
        public string EventName { get; set; }
        public string EventAreaName { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Price { get; set; }
    }
}
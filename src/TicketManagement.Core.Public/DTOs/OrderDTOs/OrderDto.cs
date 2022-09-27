namespace TicketManagement.Core.Public.DTOs.OrderDTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int EventSeatId { get; set; }
        public Guid UserId { get; set; }
        public DateTime BougthDate { get; set; }
        public decimal Price { get; set; }
    }
}
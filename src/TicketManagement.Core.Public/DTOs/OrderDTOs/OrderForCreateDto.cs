namespace TicketManagement.Core.Public.DTOs.OrderDTOs
{
    public class OrderForCreateDto
    {
        public int EventSeatId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? BoughtDate { get; set; }
        public decimal Price { get; set; }
    }
}
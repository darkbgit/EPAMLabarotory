namespace TicketManagement.Core.Public.DTOs.EventDTOs
{
    public class EventWithVenueIdDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LayoutId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ImageUrl { get; set; }

        public int VenueId { get; set; }
    }
}
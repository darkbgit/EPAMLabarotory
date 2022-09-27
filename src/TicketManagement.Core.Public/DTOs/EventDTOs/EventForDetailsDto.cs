namespace TicketManagement.Core.Public.DTOs.EventDTOs
{
    public class EventForDetailsDto
    {
        public int Id { get; set; }
        public string VenueDescription { get; set; }
        public string VenueAddress { get; set; }
        public string VenuePhone { get; set; }
        public string LayoutDescription { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan Duration { get; set; }
        public int FreeSeats { get; set; }
    }
}
#nullable enable
#pragma warning disable CS8618
namespace TicketManagement.Core.Public.DTOs.VenueDTOs
{
    public class VenueForCreateDto
    {
        public string Description { get; set; }
        public string Address { get; set; }
        public string? Phone { get; set; }
    }
}
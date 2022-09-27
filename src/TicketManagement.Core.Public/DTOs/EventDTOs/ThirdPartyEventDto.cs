namespace TicketManagement.Core.Public.DTOs.EventDTOs
{
    [Serializable]
    public class ThirdPartyEventDto
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        public string PosterImage { get; set; }

        public int LayoutId { get; set; }
    }
}
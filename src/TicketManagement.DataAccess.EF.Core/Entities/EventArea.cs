namespace TicketManagement.DataAccess.EF.Core.Entities
{
    public class EventArea : IBaseEntity
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Description { get; set; }
        public int CoordX { get; set; }
        public int CoordY { get; set; }
        public decimal? Price { get; set; }
    }
}
namespace TicketManagement.DataAccess.EF.Core.Entities
{
    /// <summary>
    /// Venue is place where Events are perform.
    /// </summary>
    public class Venue : IBaseEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string TimeZoneId { get; set; }
    }
}
namespace TicketManagement.DataAccess.EF.Core.Entities
{
    /// <summary>
    /// Layout entity.
    /// </summary>
    public class Layout : IBaseEntity
    {
        public int Id { get; set; }
        public int VenueId { get; set; }
        public string Description { get; set; }
    }
}
namespace TicketManagement.DataAccess.EF.Core.Entities
{
    /// <summary>
    /// Event entity.
    /// </summary>
    public class Event : IBaseEntity
    {
        public int Id { get; set; }
        public int LayoutId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ImageUrl { get; set; }
    }
}
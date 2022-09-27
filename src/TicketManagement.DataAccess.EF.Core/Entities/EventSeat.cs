namespace TicketManagement.DataAccess.EF.Core.Entities
{
    /// <summary>
    /// EventSeat entity.
    /// </summary>
    public class EventSeat : IBaseEntity
    {
        public int Id { get; set; }
        public int EventAreaId { get; set; }
        public int Row { get; set; }
        public int Number { get; set; }
        public int State { get; set; }
    }
}
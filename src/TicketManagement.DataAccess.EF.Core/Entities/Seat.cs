namespace TicketManagement.DataAccess.EF.Core.Entities
{
    /// <summary>
    /// Seat entity.
    /// </summary>
    public class Seat : IBaseEntity
    {
        public int Id { get; set; }
        public int AreaId { get; set; }
        public int Row { get; set; }
        public int Number { get; set; }
    }
}
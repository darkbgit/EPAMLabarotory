using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.DataAccess.EF.Core.Entities
{
    /// <summary>
    /// Order entity.
    /// </summary>
    public class Order : IBaseEntity
    {
        public int Id { get; set; }
        public int EventSeatId { get; set; }
        public Guid UserId { get; set; }
        public DateTime BoughtDate { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Price { get; set; }
    }
}
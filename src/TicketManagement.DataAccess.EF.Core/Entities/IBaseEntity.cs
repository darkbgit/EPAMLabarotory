namespace TicketManagement.DataAccess.EF.Core.Entities
{
    /// <summary>
    /// Base entity with Id.
    /// </summary>
    public interface IBaseEntity
    {
        public int Id { get; set; }
    }
}
namespace TicketManagement.MVC.Models.Queries.Events
{
    public class IdQuery : BaseQuery
    {
        public IdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
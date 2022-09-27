namespace TicketManagement.MVC.Models.Queries.Events
{
    public class EventAreasForEditListRequestQuery : BaseQuery
    {
        public int EventId { get; set; }
        public int PageSize { get; set; }
        public int? PageIndex { get; set; }
    }
}

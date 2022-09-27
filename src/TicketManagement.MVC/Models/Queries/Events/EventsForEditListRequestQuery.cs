namespace TicketManagement.MVC.Models.Queries.Events
{
    public class EventsForEditListRequestQuery : BaseQuery
    {
        public string? SortOrder { get; set; }
        public string? SearchString { get; set; }
        public int PageSize { get; set; }
        public int? PageIndex { get; set; }
    }
}
namespace TicketManagement.Core.Public.Requests
{
    public class PaginationSearchSortRequest
    {
        public string SortOrder { get; set; }
        public string SearchString { get; set; }
        public int PageSize { get; set; }
        public int? PageIndex { get; set; }
    }
}
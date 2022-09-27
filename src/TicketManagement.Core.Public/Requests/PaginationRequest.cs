namespace TicketManagement.Core.Public.Requests
{
    public class PaginationRequest
    {
        public int PageSize { get; set; }
        public int? PageIndex { get; set; }
    }
}

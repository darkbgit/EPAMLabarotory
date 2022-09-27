namespace TicketManagement.Core.Public.Responses.UserManagement
{
    public class UserInfoResponse
    {
        public Guid Id { get; set; }
        public string Locale { get; set; }
        public string TimeZoneId { get; set; }
        public List<string> Roles { get; set; }
    }
}
namespace TicketManagement.Core.Public.Requests.UserManagement
{
    public class UserForUpdateRequest
    {
        public Guid Id { get; set; }
        public string TimeZoneId { get; set; }
        public string Language { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
    }
}
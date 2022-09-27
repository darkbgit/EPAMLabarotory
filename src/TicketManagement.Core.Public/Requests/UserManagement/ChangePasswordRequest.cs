namespace TicketManagement.Core.Public.Requests.UserManagement
{
    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Core.Public.Requests.UserManagement
{
    public class LoginRequest
    {
        [Required]
        public string UsernameOrEmail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketManagement.MVC.Clients.UserManagement;

namespace TicketManagement.MVC.Areas.Admin.Pages.Manage
{
    public class UserCreateModel : PageModel
    {
        private readonly IUsersClient _usersClient;

        public UserCreateModel(IUsersClient usersClient)
        {
            _usersClient = usersClient;
        }

        public void OnGet()
        {
            // e
        }
    }
}

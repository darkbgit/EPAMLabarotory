using System.ComponentModel.DataAnnotations;

namespace TicketManagement.MVC.Models.ViewModels.Users
{
    public class UserWithRolesViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Roles")]
        public IEnumerable<string> Roles { get; set; }
    }
}

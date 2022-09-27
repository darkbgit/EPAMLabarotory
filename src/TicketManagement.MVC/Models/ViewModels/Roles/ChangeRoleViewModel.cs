using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.MVC.Models.ViewModels.Roles;

public class ChangeRoleViewModel
{
    public ChangeRoleViewModel()
    {
        AllRoles = new List<Role>();
        UserRoles = new List<string>();
    }

    public Guid UserId { get; set; }
    public string UserEmail { get; set; }
    public IEnumerable<Role> AllRoles { get; set; }
    public IList<string> UserRoles { get; set; }
}

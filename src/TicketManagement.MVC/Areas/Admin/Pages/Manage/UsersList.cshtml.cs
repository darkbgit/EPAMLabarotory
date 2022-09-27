using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Refit;
using TicketManagement.Core.Public.DTOs.UserDtos;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.Core.Public.Requests;
using TicketManagement.MVC.Clients.UserManagement;

namespace TicketManagement.MVC.Areas.Admin.Pages.Manage
{
    public class UsersListModel : PageModel
    {
        private readonly IUsersClient _usersClient;
        private readonly IStringLocalizer<UsersListModel> _stringLocalizer;

        public UsersListModel(IUsersClient usersClient,
            IStringLocalizer<UsersListModel> stringLocalizer)
        {
            _usersClient = usersClient;
            _stringLocalizer = stringLocalizer;
        }

        public PaginatedList<UserWithRolesDto> Users { get; set; }

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            const int defaultPage = 1;

            pageIndex ??= defaultPage;

            var query = new PaginationRequest
            {
                PageSize = Utilities.Constants.UsersPerAdminUsersList,
                PageIndex = pageIndex,
            };

            try
            {
                Users = await _usersClient.GetPaginatedUsers(query);
            }
            catch
            {
                return BadRequest();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDelete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                await _usersClient.DeleteUser(id.Value);
                TempData["Result"] = _stringLocalizer["DeleteSuccess"].Value;
                return RedirectToPage("./UsersList");
            }
            catch (ApiException e)
            {
                var content = await e.GetContentAsAsync<Dictionary<string, string>>();

                var message = _stringLocalizer["UnknownError"].Value;

                if (content != null)
                {
                    message = string.Join(".", content.Values);
                }

                TempData["Result"] = message;
                return RedirectToPage("./UsersList");
            }
        }
    }
}

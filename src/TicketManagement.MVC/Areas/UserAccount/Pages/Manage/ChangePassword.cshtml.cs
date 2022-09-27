#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Refit;
using TicketManagement.Core.Public.DTOs.UserDtos;
using TicketManagement.Core.Public.Extensions;
using TicketManagement.Core.Public.Requests.UserManagement;
using TicketManagement.MVC.Clients.UserManagement;

namespace TicketManagement.MVC.Areas.UserAccount.Pages.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IUsersClient _usersClient;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ChangePasswordModel(ILogger<ChangePasswordModel> logger,
            IUsersClient usersClient)
        {
            _logger = logger;
            _usersClient = usersClient;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _usersClient.GetUser();
            if (user == null)
            {
                return NotFound("Unable to load user.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            UserDto user;

            try
            {
                user = await _usersClient.GetUser();
            }
            catch (ApiException)
            {
                return NotFound($"Unable to load user.");
            }

            if (user == null)
            {
                return NotFound($"Unable to load user.");
            }

            var request = new ChangePasswordRequest
            {
                OldPassword = Input.OldPassword,
                NewPassword = Input.NewPassword,
            };

            try
            {
                await _usersClient.ChangePasword(user.Id, request);
            }
            catch (ApiException e)
            {
                _logger.LogError("{e}", e);
                e.ErrorsToModelStateErrors(this);
                return Page();
            }

            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Your password has been changed.";

            return RedirectToPage();
        }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
    }
}

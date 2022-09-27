#nullable disable
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Refit;
using TicketManagement.Core.Public.DTOs.UserDtos;
using TicketManagement.Core.Public.Extensions;
using TicketManagement.Core.Public.Requests.UserManagement;
using TicketManagement.MVC.Clients.UserManagement;

namespace TicketManagement.MVC.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IUsersClient _usersClient;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(ILogger<RegisterModel> logger,
            IUsersClient usersClient)
        {
            _logger = logger;
            _usersClient = usersClient;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userForCreate = new UserForCreateDto
            {
                UserName = Input.Name,
                Email = Input.Email,
                Password = Input.Password,
            };

            UserDto user;

            try
            {
                user = await _usersClient.CreateUser(userForCreate);
            }
            catch (ApiException e)
            {
                _logger.LogError("{e}", e);
                e.ErrorsToModelStateErrors(this);
                return Page();
            }

            _logger.LogInformation("User created a new account with password.");

            var addToRoleRequest = new AddToRoleRequest
            {
                Role = Core.Public.Enums.Roles.User.ToString(),
            };

            try
            {
                await _usersClient.AddToRole(user.Id, addToRoleRequest);
            }
            catch (ApiException e)
            {
                _logger.LogError("{e}", e);
                e.ErrorsToModelStateErrors(this);
                return Page();
            }

            return RedirectToPage("./Login", new { returnUrl });
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }
        }
    }
}

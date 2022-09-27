#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Refit;
using TicketManagement.Core.Public.Requests.UserManagement;
using TicketManagement.MVC.Clients.UserManagement;

namespace TicketManagement.MVC.Areas.UserAccount.Pages.Manage
{
    public class BalanceModel : PageModel
    {
        private readonly IUsersClient _usersClient;

        public BalanceModel(IUsersClient usersClient)
        {
            _usersClient = usersClient;
        }

        [BindProperty]
        [Display(Name = "Balance")]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var id = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;

            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var guidId))
            {
                return Unauthorized();
            }

            try
            {
                var balance = await _usersClient.GetUserBalance(guidId);
                Balance = balance;
            }
            catch (ApiException)
            {
                return BadRequest();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var id = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;

            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var guidId))
            {
                return Unauthorized();
            }

            var request = new TopUpBalanceRequest
            {
                AdditionBalance = Input.AddBalance,
            };

            try
            {
                await _usersClient.TopUpUserBalance(guidId, request);
            }
            catch (ApiException)
            {
                StatusMessage = "Unexpected error when trying to update balance.";
                return RedirectToPage();
            }

            StatusMessage = "Your balance has been updated";
            return RedirectToPage();
        }

        public class InputModel
        {
            [DataType(DataType.Currency)]
            [Range(0, int.MaxValue, ErrorMessage = "Value error")]
            public decimal AddBalance { get; set; }
        }
    }
}
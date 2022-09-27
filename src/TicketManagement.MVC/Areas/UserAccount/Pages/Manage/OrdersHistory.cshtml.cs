#nullable disable
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Refit;
using TicketManagement.Core.Public.DTOs.OrderDTOs;
using TicketManagement.MVC.Clients.OrderManagement;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace TicketManagement.MVC.Areas.UserAccount.Pages.Manage
{
    [Authorize]
    public class OrdersHistoryModel : PageModel
    {
        private readonly IOrdersClient _ordersClient;

        public OrdersHistoryModel(IOrdersClient ordersClient)
        {
            _ordersClient = ordersClient;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public IEnumerable<OrderTicketDto> Orders { get; set; } = new List<OrderTicketDto>();

        public async Task<IActionResult> OnGet()
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
                var orders = await _ordersClient.GetOrdersByUserId(guidId);
                Orders = orders;
            }
            catch (ApiException)
            {
                return BadRequest();
            }

            return Page();
        }
    }
}

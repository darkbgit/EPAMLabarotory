using Microsoft.AspNetCore.Mvc;
using TicketManagement.Core.OrderManagement.Services.Interfaces;
using TicketManagement.Core.Public.DTOs.OrderDTOs;
using TicketManagement.Core.Public.Enums;
using TicketManagement.Core.Public.Filters;

namespace TicketManagement.OrderManagement.API.Controllers
{
    [Route("api/order-management/[controller]")]
    [ApiController]
    [Authorize(Roles.User)]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Get all orders by user id.
        /// </summary>
        [HttpGet("with-ticket-info-by-user-id/{id}")]
        public async Task<ActionResult<IEnumerable<OrderTicketDto>>> GetOrders(Guid id)
        {
            var isAuthenticated = HttpContext?.User.Identity is { IsAuthenticated: true };

            if (!isAuthenticated)
            {
                return Forbid();
            }

            var orders = await _orderService.GetOrdersByUserId(id);

            return Ok(orders);
        }

        /// <summary>
        /// Create order.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<OrderTicketDto>> CreateOrder([FromBody] OrderForCreateDto dto)
        {
            var result = await _orderService.CreateAsync(dto);

            return Ok(result);
        }
    }
}

using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Refit;
using TicketManagement.Core.Public.DTOs.OrderDTOs;
using TicketManagement.MVC.Clients.OrderManagement;
using TicketManagement.MVC.Models.ViewModels.Order;

namespace TicketManagement.MVC.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class OrderController : Controller
    {
        private readonly IOrdersClient _ordersClient;
        private readonly IMapper _mapper;

        public OrderController(IMapper mapper,
           IOrdersClient ordersClient)
        {
            _mapper = mapper;
            _ordersClient = ordersClient;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int[] eventSeatsIds)
        {
            if (!eventSeatsIds.Any())
            {
                return BadRequest();
            }

            var id = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;

            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var guidId))
            {
                return Unauthorized();
            }

            var model = new List<TicketsBougthViewModel>();

            foreach (var eventSeatId in eventSeatsIds)
            {
                var orderForCreateDto = new OrderForCreateDto
                {
                    EventSeatId = eventSeatId,
                    UserId = guidId,
                    BoughtDate = DateTime.UtcNow,
                };

                try
                {
                    var result = await _ordersClient.CreateOrder(orderForCreateDto);

                    model.Add(_mapper.Map<TicketsBougthViewModel>(result));
                }
                catch (ApiException e)
                {
                    ModelState.AddModelError("Model", e.Message);
                }
            }

            return PartialView("OrderComplete", model);
        }
    }
}

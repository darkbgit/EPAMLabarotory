using Refit;
using TicketManagement.Core.Public.DTOs.OrderDTOs;

namespace TicketManagement.MVC.Clients.OrderManagement
{
    public interface IOrdersClient
    {
        [Get("/orders/with-ticket-info-by-user-id/{id}")]
        Task<List<OrderTicketDto>> GetOrdersByUserId(Guid id);

        [Post("/orders")]
        Task<OrderTicketDto> CreateOrder([Body] OrderForCreateDto dto);
    }
}

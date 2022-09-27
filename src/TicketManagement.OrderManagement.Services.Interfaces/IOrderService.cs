using TicketManagement.Core.Public.DTOs.OrderDTOs;

namespace TicketManagement.Core.OrderManagement.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderTicketDto> CreateAsync(OrderForCreateDto orderForCreateDto, CancellationToken cancellationToken = default);

        Task<IEnumerable<OrderTicketDto>> GetOrdersByUserId(Guid userId, CancellationToken cancellationToken = default);
    }
}
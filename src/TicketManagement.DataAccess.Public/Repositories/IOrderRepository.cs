using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketManagement.Core.Public.DTOs.OrderDTOs;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.DataAccess.Public.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<OrderTicketDto> GetTicketInfoByIdAsync(int orderId, CancellationToken cancellationToken);

        IQueryable<OrderTicketDto> GetOrdersByUserId(Guid userId);

        void CreateOrder(Order entity);
    }
}
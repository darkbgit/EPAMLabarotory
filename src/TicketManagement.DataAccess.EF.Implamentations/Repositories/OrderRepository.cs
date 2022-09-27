using Microsoft.EntityFrameworkCore;
using TicketManagement.Core.Public.DTOs.OrderDTOs;
using TicketManagement.DataAccess.EF.Core;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.EF.Implementation.Repositories.Base;
using TicketManagement.DataAccess.Public.Repositories;

namespace TicketManagement.DataAccess.EF.Implementation.Repositories
{
    internal sealed class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(TicketManagementContext context)
            : base(context)
        {
        }

        public void CreateOrder(Order entity)
        {
            Add(entity);
        }

        public IQueryable<OrderTicketDto> GetOrdersByUserId(Guid userId)
        {
            var query = from order in Table
                        where order.UserId == userId
                        join eventSeat in Context.EventSeat on order.EventSeatId equals eventSeat.Id
                        join eventArea in Context.EventArea on eventSeat.EventAreaId equals eventArea.Id
                        join @event in Context.Event on eventArea.EventId equals @event.Id
                        join layout in Context.Layout on @event.LayoutId equals layout.Id
                        join venue in Context.Venue on layout.VenueId equals venue.Id
                        select new OrderTicketDto
                        {
                            Id = order.Id,
                            Row = eventSeat.Row,
                            Number = eventSeat.Number,
                            EventAreaName = eventArea.Description,
                            Price = eventArea.Price ?? default,
                            EventName = @event.Name,
                            StartDate = @event.StartDate,
                            LayoutName = layout.Description,
                            VenueName = venue.Description,
                        };

            return query;
        }

        public async Task<OrderTicketDto> GetTicketInfoByIdAsync(int orderId, CancellationToken cancellationToken)
        {
            var query = from order in Table
                        where order.Id == orderId
                        join eventSeat in Context.EventSeat on order.EventSeatId equals eventSeat.Id
                        join eventArea in Context.EventArea on eventSeat.EventAreaId equals eventArea.Id
                        join @event in Context.Event on eventArea.EventId equals @event.Id
                        join layout in Context.Layout on @event.LayoutId equals layout.Id
                        join venue in Context.Venue on layout.VenueId equals venue.Id
                        select new OrderTicketDto
                        {
                            Id = order.Id,
                            Row = eventSeat.Row,
                            Number = eventSeat.Number,
                            EventAreaName = eventArea.Description,
                            Price = eventArea.Price ?? default,
                            EventName = @event.Name,
                            StartDate = @event.StartDate,
                            LayoutName = layout.Description,
                            VenueName = venue.Description,
                        };

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
using Microsoft.EntityFrameworkCore;
using TicketManagement.Core.Public.DTOs.EventAreaDTOs;
using TicketManagement.Core.Public.Enums;
using TicketManagement.DataAccess.EF.Core;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.EF.Implementation.Repositories.Base;
using TicketManagement.DataAccess.Public.Repositories;

namespace TicketManagement.DataAccess.EF.Implementation.Repositories;

internal sealed class EventAreaRepository : Repository<EventArea>, IEventAreaRepository
{
    public EventAreaRepository(TicketManagementContext context)
        : base(context)
    {
    }

    public IQueryable<EventArea> GetEventAreasByEventId(int eventId)
    {
        return Table.Where(eventArea => eventArea.EventId == eventId);
    }

    public void CreateEventArea(EventArea entity)
    {
        Add(entity);
    }

    public void UpdateEventArea(EventArea entity)
    {
        Update(entity);
    }

    public void DeleteEventArea(EventArea entity)
    {
        Remove(entity);
    }

    public async Task<bool> IsExistAsync(EventArea entity, CancellationToken cancellationToken)
    {
        var result = await Table.Where(eventArea => eventArea.EventId == entity.EventId)
            .Where(eventArea =>
                eventArea.Description == entity.Description || (eventArea.CoordX == entity.CoordX && eventArea.CoordY == entity.CoordY))
            .FirstOrDefaultAsync(cancellationToken);

        return result != null;
    }

    public async Task<bool> IsAnotherExistAsync(EventArea entity, CancellationToken cancellationToken)
    {
        var result = await Table
            .Where(eventArea => eventArea.Id != entity.Id && eventArea.EventId == entity.EventId)
            .Where(eventArea =>
                eventArea.Description == entity.Description || (eventArea.CoordX == entity.CoordX && eventArea.CoordY == entity.CoordY))
            .FirstOrDefaultAsync(cancellationToken);

        return result != null;
    }

    public IQueryable<EventAreaWithSeatsNumberDto> GetEventAreasWithSeatsCountByEventId(int eventId)
    {
        var query = from eventArea in Table.Where(e => e.EventId.Equals(eventId))
                    join eventSeat in Context.EventSeat on eventArea.Id equals eventSeat.EventAreaId
                    group eventArea by new
                    {
                        eventArea.Id,
                        eventArea.Description,
                        eventArea.EventId,
                        eventArea.CoordX,
                        eventArea.CoordY,
                        eventArea.Price,
                    }
            into grouping
                    select new EventAreaWithSeatsNumberDto
                    {
                        Id = grouping.Key.Id,
                        EventId = grouping.Key.EventId,
                        CoordX = grouping.Key.CoordX,
                        CoordY = grouping.Key.CoordY,
                        Description = grouping.Key.Description,
                        Price = grouping.Key.Price,
                        TotalSeats = grouping.Count(),
                    };

        return query;
    }

    public IQueryable<EventAreaWithSeatsAndFreeSeatsCountDto> GetEventAreasWithSeatsAndFreeSeatsCountByEventId(int eventId)
    {
        var query = from eventArea in Table.Where(e => e.EventId.Equals(eventId))
                    join eventSeat in Context.EventSeat on eventArea.Id equals eventSeat.EventAreaId into eventSeats
                    from eventSeat in eventSeats.DefaultIfEmpty()
                    group eventSeat by new
                    {
                        eventArea.Id,
                        eventArea.Description,
                        eventArea.EventId,
                        eventArea.CoordX,
                        eventArea.CoordY,
                        eventArea.Price,
                    }
            into grouping
                    select new EventAreaWithSeatsAndFreeSeatsCountDto
                    {
                        Id = grouping.Key.Id,
                        EventId = grouping.Key.EventId,
                        CoordX = grouping.Key.CoordX,
                        CoordY = grouping.Key.CoordY,
                        Description = grouping.Key.Description,
                        Price = grouping.Key.Price.HasValue ? grouping.Key.Price.Value : default,
                        TotalSeats = grouping.Count(),
                        FreeSeats = grouping.Count(es => es != null && es.State == (int)SeatState.Free),
                    };

        return query;
    }
}
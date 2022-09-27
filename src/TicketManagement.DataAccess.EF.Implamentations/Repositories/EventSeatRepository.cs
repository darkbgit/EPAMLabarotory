using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.EF.Core;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.EF.Implementation.Repositories.Base;
using TicketManagement.DataAccess.Public.Repositories;

namespace TicketManagement.DataAccess.EF.Implementation.Repositories;

internal sealed class EventSeatRepository : Repository<EventSeat>, IEventSeatRepository
{
    public EventSeatRepository(TicketManagementContext context)
        : base(context)
    {
    }

    public IQueryable<EventSeat> GetEventSeatsByEventAreaId(int eventAreaId)
    {
        return Table.Where(eventSeat => eventSeat.EventAreaId == eventAreaId);
    }

    public async Task UpdateStateAsync(int eventSeatId, int newState, CancellationToken cancellationToken)
    {
        var eventSeat = await Table.FindAsync(eventSeatId);

        if (eventSeat == null)
        {
            throw new InvalidOperationException("Invalid EventSeat Id");
        }

        eventSeat.State = newState;

        Update(eventSeat);
    }

    public async Task<bool> IsExistAsync(EventSeat entity, CancellationToken cancellationToken)
    {
        var eventSeat = await Table
            .Where(eventSeat => eventSeat.Id == entity.Id && eventSeat.EventAreaId == entity.EventAreaId && eventSeat.Row == entity.Row && eventSeat.Number == entity.Number)
            .FirstOrDefaultAsync(cancellationToken);

        return eventSeat != null;
    }

    public async Task<bool> IsAnotherExistAsync(EventSeat entity, CancellationToken cancellationToken)
    {
        var eventSeat = await Table
            .Where(eventSeat => eventSeat.Id != entity.Id && eventSeat.EventAreaId == entity.EventAreaId && eventSeat.Row == entity.Row && eventSeat.Number == entity.Number)
            .FirstOrDefaultAsync(cancellationToken);

        return eventSeat != null;
    }

    public async Task<bool> IsExistWithStateAsync(int eventSeatId, int seatState, CancellationToken cancellationToken)
    {
        var eventSeat = await Table
            .FirstOrDefaultAsync(seat => seat.Id == eventSeatId && seat.State == seatState, cancellationToken);

        return eventSeat != null;
    }

    public void AddEventSeatsRange(IEnumerable<EventSeat> eventSeats)
    {
        AddRange(eventSeats);
    }

    public void CreateEventSeat(EventSeat entity)
    {
        Add(entity);
    }

    public void UpdateEventSeat(EventSeat entity)
    {
        Update(entity);
    }

    public void DeleteEventSeat(EventSeat entity)
    {
        Remove(entity);
    }
}
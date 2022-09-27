using System.Data.Common;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TicketManagement.Core.EventManagement.Services.Interfaces;
using TicketManagement.Core.Public.DTOs.EventAreaDTOs;
using TicketManagement.Core.Public.Enums;
using TicketManagement.Core.Public.Exceptions;
using TicketManagement.Core.Public.Localization;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.Public;

namespace TicketManagement.Core.EventManagement.Services;

internal sealed class EventAreaService : IEventAreaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<EventArea> _validator;
    private readonly IStringLocalizer<SharedResource> _stringLocalizer;
    private readonly ILogger<EventAreaService> _logger;

    public EventAreaService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<EventArea> validator,
        ILogger<EventAreaService> logger,
        IStringLocalizer<SharedResource> stringLocalizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
        _logger = logger;
        _stringLocalizer = stringLocalizer;
    }

    public async Task<IEnumerable<EventAreaWithSeatsAndFreeSeatsCountDto>> GetEventAreasWithSeatsInfoByEventIdAsync(int eventId, CancellationToken cancellationToken = default)
    {
        var eventAreas = await _unitOfWork.EventArea.GetEventAreasWithSeatsAndFreeSeatsCountByEventId(eventId)
            .ToListAsync(cancellationToken);

        return eventAreas;
    }

    public async Task<PaginatedList<EventAreaWithSeatsNumberDto>> GetPagedEventAreasByEventIdAsync(int eventId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var eventAreas = await _unitOfWork.EventArea.GetEventAreasWithSeatsCountByEventId(eventId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var count = await _unitOfWork.EventArea
            .FindBy(eventArea => eventArea.EventId == eventId)
            .CountAsync(cancellationToken);

        var result = new PaginatedList<EventAreaWithSeatsNumberDto>(eventAreas,
            count,
            pageNumber,
            pageSize);

        return result;
    }

    public async Task<int> GetEventIdByEventAreaIdAsync(int eventAreaId, CancellationToken cancellationToken)
    {
        var id = await _unitOfWork.EventArea
            .FindBy(eventArea => eventArea.Id == eventAreaId)
            .Select(eventArea => eventArea.EventId)
            .FirstOrDefaultAsync(cancellationToken);

        if (id == 0)
        {
            throw new ServiceException(_stringLocalizer["error.NotFoundSimple", nameof(EventArea)]);
        }

        return id;
    }

    public async Task<EventAreaDto?> GetByIdAsync(int eventAreaId, CancellationToken cancellationToken = default)
    {
        var eventArea = await _unitOfWork.EventArea.GetByIdAsync(eventAreaId, cancellationToken);

        var result = _mapper.Map<EventAreaDto>(eventArea);

        return result;
    }

    public async Task UpdateAsync(EventAreaDto dto, CancellationToken cancellationToken = default)
    {
        var eventArea = await _unitOfWork.EventArea.GetByIdAsync(dto.Id, cancellationToken);

        if (eventArea is null)
        {
            throw new ServiceException(_stringLocalizer["error.Update", nameof(EventArea)]);
        }

        _mapper.Map(dto, eventArea);

        await _validator.ValidateAndThrowAsync(eventArea, cancellationToken);

        var isExist = await _unitOfWork.EventArea.IsAnotherExistAsync(eventArea, cancellationToken);

        if (isExist)
        {
            throw new ServiceException(_stringLocalizer["error.IsExist", eventArea.Description, eventArea.CoordX, eventArea.CoordY]);
        }

        _unitOfWork.EventArea.UpdateEventArea(eventArea);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbException e)
        {
            throw new ServiceException(_stringLocalizer["error.Update", nameof(EventArea)], e);
        }
    }

    public async Task DeleteAsync(int eventAreaId, CancellationToken cancellationToken = default)
    {
        var eventArea = await _unitOfWork.EventArea.GetByIdAsync(eventAreaId, cancellationToken);

        if (eventArea is null)
        {
            throw new ServiceException(_stringLocalizer["error.Delete", nameof(EventArea)]);
        }

        var seats = await _unitOfWork.EventSeat
            .GetEventSeatsByEventAreaId(eventAreaId)
            .ToListAsync(cancellationToken);

        if (seats.Any(seat => seat.State == (int)SeatState.Occupied))
        {
            throw new ServiceException(_stringLocalizer["error.EventAreaDelete.HaveOccupiedSeats", nameof(EventArea)]);
        }

        _unitOfWork.EventArea.DeleteEventArea(eventArea);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbException e)
        {
            throw new ServiceException(_stringLocalizer["error.Delete", nameof(EventArea)], e);
        }
    }
}
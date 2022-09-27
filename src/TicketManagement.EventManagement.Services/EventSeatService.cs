using System.Data.Common;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TicketManagement.Core.EventManagement.Services.Interfaces;
using TicketManagement.Core.Public.DTOs.EventSeatDTOs;
using TicketManagement.Core.Public.Exceptions;
using TicketManagement.Core.Public.Localization;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.Public;

namespace TicketManagement.Core.EventManagement.Services
{
    internal sealed class EventSeatService : IEventSeatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<EventSeat> _validator;
        private readonly IStringLocalizer<SharedResource> _serviceResourcesStringLocalizer;
        private readonly IStringLocalizer<EventSeatService> _stringLocalizer;

        public EventSeatService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<EventSeat> validator,
            IStringLocalizer<SharedResource> serviceResourcesStringLocalizer,
            IStringLocalizer<EventSeatService> stringLocalizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
            _serviceResourcesStringLocalizer = serviceResourcesStringLocalizer;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<IEnumerable<EventSeatForListDto>> GetAllEventSeatsByEventAreaIdAsync(int eventAreaId, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.EventSeat
                .FindBy(es => es.EventAreaId.Equals(eventAreaId))
                .ToListAsync(cancellationToken);

            return result.Select(eventSeat => _mapper.Map<EventSeatForListDto>(eventSeat)).ToList();
        }

        public async Task<EventSeatDto?> GetByIdAsync(int eventSeatId, CancellationToken cancellationToken = default)
        {
            var eventSeat = await _unitOfWork.EventSeat
                .GetByIdAsync(eventSeatId, cancellationToken);

            var result = _mapper.Map<EventSeatDto>(eventSeat);

            return result;
        }

        public async Task<PaginatedList<EventSeatDto>> GetPagedEventSeatsByEventAreaIdAsync(int eventAreaId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.EventSeat.GetEventSeatsByEventAreaId(eventAreaId);

            var eventSeats = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var eventSeatsDto = eventSeats
                .Select(es => _mapper.Map<EventSeatDto>(es))
                .ToList();

            var count = await query.CountAsync(cancellationToken);

            var result = new PaginatedList<EventSeatDto>(eventSeatsDto,
                count,
                pageNumber,
                pageSize);

            return result;
        }

        public async Task UpdateAsync(EventSeatDto dto, CancellationToken cancellationToken = default)
        {
            var eventSeat = await _unitOfWork.EventSeat.GetByIdAsync(dto.Id, cancellationToken);

            if (eventSeat is null)
            {
                throw new ServiceException(_serviceResourcesStringLocalizer["error.Update", nameof(EventSeat)]);
            }

            _mapper.Map(dto, eventSeat);

            await _validator.ValidateAndThrowAsync(eventSeat, cancellationToken);

            var isExist = await _unitOfWork.EventSeat.IsAnotherExistAsync(eventSeat, cancellationToken);

            if (isExist)
            {
                throw new ServiceException(_stringLocalizer["error.IsExist", eventSeat.Row, eventSeat.Number]);
            }

            _unitOfWork.EventSeat.UpdateEventSeat(eventSeat);

            try
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (DbException e)
            {
                throw new ServiceException(_serviceResourcesStringLocalizer["error.Update", nameof(EventSeat)], e);
            }
        }

        public async Task DeleteAsync(int eventSeatId, CancellationToken cancellationToken = default)
        {
            var eventSeat = await _unitOfWork.EventSeat.GetByIdAsync(eventSeatId, cancellationToken);

            if (eventSeat is null)
            {
                throw new ServiceException(_serviceResourcesStringLocalizer["error.Delete", nameof(EventSeat)]);
            }

            _unitOfWork.EventSeat.DeleteEventSeat(eventSeat);

            try
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (DbException e)
            {
                throw new ServiceException(_serviceResourcesStringLocalizer["error.Delete", nameof(EventSeat)], e);
            }
        }
    }
}
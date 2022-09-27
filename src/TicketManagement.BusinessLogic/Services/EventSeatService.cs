using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using TicketManagement.BusinessLogic.Task1.Exceptions;
using TicketManagement.BusinessLogic.Task1.Models;
using TicketManagement.BusinessLogic.Task1.Services.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.DataAccess.Repositories.Interfaces;

namespace TicketManagement.BusinessLogic.Task1.Services
{
    internal class EventSeatService : IEventSeatService
    {
        private readonly IEventSeatRepository _eventSeatRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<EventSeatDto> _eventSeatValidator;
        public EventSeatService(IEventSeatRepository eventSeatRepository,
            IValidator<EventSeatDto> eventSeatValidator,
            IMapper mapper)
        {
            _eventSeatRepository = eventSeatRepository;
            _mapper = mapper;
            _eventSeatValidator = eventSeatValidator;
        }

        public async Task<EventSeatDto> GetByIdAsync(int id)
        {
            var result = await _eventSeatRepository.GetByIdAsync(id);

            return _mapper.Map<EventSeatDto>(result);
        }

        public async Task<IEnumerable<EventSeatDto>> GetEventSeatsByEventAreaId(int eventAreaId)
        {
            var result = await _eventSeatRepository.GetSeatsByAreaIdAsync(eventAreaId);

            return result.Select(s => _mapper.Map<EventSeatDto>(s));
        }

        public async Task<bool> AddAsync(EventSeatDto eventSeat)
        {
            await _eventSeatValidator.ValidateAndThrowAsync(eventSeat);

            var eventSeatEntity = _mapper.Map<EventSeat>(eventSeat);

            if (await _eventSeatRepository.IsExists(eventSeatEntity))
            {
                throw new ServiceException(
                    $"EventSeat with the same params already exists in DB. Check Row - {eventSeatEntity.Row} and Number - {eventSeatEntity.Number} must be unique in Area.");
            }

            try
            {
                var id = await _eventSeatRepository.AddAsync(eventSeatEntity);
                return id > 0;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while adding {nameof(EventSeat)} in database.", e);
            }
        }

        public async Task<bool> RemoveAsync(int id)
        {
            try
            {
                var result = await _eventSeatRepository.RemoveAsync(id);
                return result == 1;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while removing {nameof(EventSeat)} from database.", e);
            }
        }

        public async Task<bool> UpdateAsync(EventSeatDto eventSeat)
        {
            await _eventSeatValidator.ValidateAndThrowAsync(eventSeat);

            var eventSeatEntity = _mapper.Map<EventSeat>(eventSeat);

            if (await _eventSeatRepository.IsAnotherExists(eventSeatEntity))
            {
                throw new ServiceException(
                    $"EventSeat with the same params already exists in DB. Check Row - {eventSeatEntity.Row} and Number - {eventSeatEntity.Number} must be unique in Area.");
            }

            try
            {
                var result = await _eventSeatRepository.UpdateAsync(eventSeatEntity);
                return result == 1;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while updating {nameof(EventSeat)} in database.", e);
            }
        }
    }
}
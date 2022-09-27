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
    internal class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<EventDto> _eventValidator;

        public EventService(IEventRepository eventRepository,
            IValidator<EventDto> eventValidator,
            IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _eventValidator = eventValidator;
        }

        public async Task<EventDto> GetByIdAsync(int id)
        {
            var result = await _eventRepository.GetByIdAsync(id);

            return _mapper.Map<EventDto>(result);
        }

        public async Task<IEnumerable<EventDto>> GetEventsByLayout(int layoutId)
        {
            var result = await _eventRepository.GetEventsByLayoutId(layoutId);

            return result.Select(e => _mapper.Map<EventDto>(e));
        }

        public async Task<bool> AddAsync(EventDto eventDto)
        {
            await _eventValidator.ValidateAndThrowAsync(eventDto);

            var eventEntity = _mapper.Map<Event>(eventDto);

            if (await _eventRepository.IsExists(eventEntity))
            {
                throw new ServiceException(
                    "Event with the same params already exists in DB." +
                    $"Check Description - {eventEntity.Description} and Name - {eventEntity.Name}, time period StartDate - {eventEntity.StartDate} EndDate - {eventEntity.EndDate} " +
                    "must be unique in Layout.");
            }

            try
            {
                var id = await _eventRepository.AddAsync(eventEntity);
                return id > 0;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while adding {nameof(Event)} in database.", e);
            }
        }

        public async Task<bool> RemoveAsync(int eventId)
        {
            try
            {
                var result = await _eventRepository.RemoveAsync(eventId);
                return result == 1;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while removing {nameof(Event)} from database.", e);
            }
        }

        public async Task<bool> UpdateAsync(EventDto eventDto)
        {
            await _eventValidator.ValidateAndThrowAsync(eventDto);

            var eventEntity = _mapper.Map<Event>(eventDto);

            if (await _eventRepository.IsAnotherExists(eventEntity))
            {
                throw new ServiceException(
                    "Event with the same params already exists in DB." +
                    $"Check Description - {eventEntity.Description} and Name - {eventEntity.Name}, time period StartDate - {eventEntity.StartDate} EndDate - {eventEntity.EndDate} " +
                    "must be unique in Layout.");
            }

            try
            {
                var result = await _eventRepository.UpdateAsync(eventEntity);
                return result == 1;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while updating {nameof(Area)} in database.", e);
            }
        }
    }
}
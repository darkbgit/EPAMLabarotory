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
    internal class EventAreaService : IEventAreaService
    {
        private readonly IEventAreaRepository _eventAreaRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<EventAreaDto> _eventAreaValidator;

        public EventAreaService(IEventAreaRepository eventAreaRepository,
            IValidator<EventAreaDto> eventAreaValidator,
            IMapper mapper)
        {
            _eventAreaRepository = eventAreaRepository;
            _mapper = mapper;
            _eventAreaValidator = eventAreaValidator;
        }

        public async Task<EventAreaDto> GetByIdAsync(int id)
        {
            var result = await _eventAreaRepository.GetByIdAsync(id);

            return _mapper.Map<EventAreaDto>(result);
        }

        public async Task<IEnumerable<EventAreaDto>> GetEventAreasByEventId(int eventId)
        {
            var result = await _eventAreaRepository.GetAreasByEventIdAsync(eventId);

            return result.Select(l => _mapper.Map<EventAreaDto>(l));
        }

        public async Task<bool> AddAsync(EventAreaDto eventArea)
        {
            await _eventAreaValidator.ValidateAndThrowAsync(eventArea);

            var eventAreaEntity = _mapper.Map<EventArea>(eventArea);

            if (await _eventAreaRepository.IsExists(eventAreaEntity))
            {
                throw new ServiceException(
                    "EventArea with the same params already exists in DB." +
                    $"Check EventArea Description - {eventAreaEntity.Description} and coord CordX - {eventAreaEntity.CoordX}, CoordY - {eventAreaEntity.CoordY} must be unique in Layout.");
            }

            try
            {
                var id = await _eventAreaRepository.AddAsync(eventAreaEntity);
                return id > 0;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while adding {nameof(EventArea)} in database.", e);
            }
        }

        public async Task<bool> RemoveAsync(int id)
        {
            try
            {
                var result = await _eventAreaRepository.RemoveAsync(id);
                return result == 1;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while removing {nameof(EventArea)} from database.", e);
            }
        }

        public async Task<bool> UpdateAsync(EventAreaDto eventArea)
        {
            await _eventAreaValidator.ValidateAndThrowAsync(eventArea);

            var eventAreaEntity = _mapper.Map<EventArea>(eventArea);

            if (await _eventAreaRepository.IsAnotherExists(eventAreaEntity))
            {
                throw new ServiceException(
                   "EventArea with the same params already exists in DB." +
                    $"Check EventArea Description - {eventAreaEntity.Description} and coord CordX - {eventAreaEntity.CoordX}, CoordY - {eventAreaEntity.CoordY} must be unique in Layout.");
            }

            try
            {
                var result = await _eventAreaRepository.UpdateAsync(eventAreaEntity);
                return result == 1;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while updating {nameof(EventArea)} in database.", e);
            }
        }
    }
}
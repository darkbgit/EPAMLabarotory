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
    internal class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<SeatDto> _seatValidator;
        public SeatService(ISeatRepository seatRepository,
            IValidator<SeatDto> seatValidator,
            IMapper mapper)
        {
            _seatRepository = seatRepository;
            _mapper = mapper;
            _seatValidator = seatValidator;
        }

        public async Task<SeatDto> GetByIdAsync(int id)
        {
            var result = await _seatRepository.GetByIdAsync(id);

            return _mapper.Map<SeatDto>(result);
        }

        public async Task<IEnumerable<SeatDto>> GetSeatsByAreaId(int areaId)
        {
            var result = await _seatRepository.GetSeatsByAreaIdAsync(areaId);

            return result.Select(s => _mapper.Map<SeatDto>(s));
        }

        public async Task<bool> AddAsync(SeatDto seat)
        {
            await _seatValidator.ValidateAndThrowAsync(seat);

            var seatEntity = _mapper.Map<Seat>(seat);

            if (await _seatRepository.IsExists(seatEntity))
            {
                throw new ServiceException(
                    $"Seat with the same params already exists in DB. Check Row - {seatEntity.Row} and Number - {seatEntity.Number} must be unique in Area.");
            }

            try
            {
                var id = await _seatRepository.AddAsync(seatEntity);
                return id > 0;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while adding {nameof(Seat)} in database.", e);
            }
        }

        public async Task<bool> RemoveAsync(int seatId)
        {
            try
            {
                var result = await _seatRepository.RemoveAsync(seatId);
                return result == 1;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while removing {nameof(Seat)} from database.", e);
            }
        }

        public async Task<bool> UpdateAsync(SeatDto seat)
        {
            await _seatValidator.ValidateAndThrowAsync(seat);

            var seatEntity = _mapper.Map<Seat>(seat);

            if (await _seatRepository.IsExists(seatEntity))
            {
                throw new ServiceException(
                    $"Seat with the same params already exists in DB. Check Row - {seatEntity.Row} and Number - {seatEntity.Number} must be unique in Area.");
            }

            try
            {
                var result = await _seatRepository.UpdateAsync(seatEntity);
                return result == 1;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while updating {nameof(Seat)} in database.", e);
            }
        }
    }
}
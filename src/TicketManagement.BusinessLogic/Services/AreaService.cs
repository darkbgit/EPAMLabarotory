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
    internal class AreaService : IAreaService
    {
        private readonly IAreaRepository _areaRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<AreaDto> _areaValidator;

        public AreaService(IAreaRepository areaRepository,
            IValidator<AreaDto> areaValidator,
            IMapper mapper)
        {
            _areaRepository = areaRepository;
            _mapper = mapper;
            _areaValidator = areaValidator;
        }

        public async Task<AreaDto> GetByIdAsync(int id)
        {
            var result = await _areaRepository.GetByIdAsync(id);

            return _mapper.Map<AreaDto>(result);
        }

        public async Task<IEnumerable<AreaDto>> GetAreasByLayoutId(int layoutId)
        {
            var result = await _areaRepository.GetAreasByLayoutIdAsync(layoutId);

            return result.Select(l => _mapper.Map<AreaDto>(l));
        }

        public async Task<bool> AddAsync(AreaDto area)
        {
            await _areaValidator.ValidateAndThrowAsync(area);

            var areaEntity = _mapper.Map<Area>(area);

            if (await _areaRepository.IsExists(areaEntity))
            {
                throw new ServiceException(
                    "Area with the same params already exists in DB." +
                    $"Check Area Description - {areaEntity.Description} and coord CordX - {areaEntity.CoordX}, CoordY - {areaEntity.CoordY} must be unique in Layout.");
            }

            try
            {
                var id = await _areaRepository.AddAsync(areaEntity);
                return id > 0;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while adding {nameof(Area)} in database.", e);
            }
        }

        public async Task<bool> RemoveAsync(int areaId)
        {
            try
            {
                var result = await _areaRepository.RemoveAsync(areaId);
                return result == 1;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while removing {nameof(Area)} from database.", e);
            }
        }

        public async Task<bool> UpdateAsync(AreaDto area)
        {
            await _areaValidator.ValidateAndThrowAsync(area);

            var areaEntity = _mapper.Map<Area>(area);

            if (await _areaRepository.IsAnotherExists(areaEntity))
            {
                throw new ServiceException(
                   "Area with the same params already exists in DB." +
                    $"Check Area Description - {areaEntity.Description} and coord CordX - {areaEntity.CoordX}, CoordY - {areaEntity.CoordY} must be unique in Layout.");
            }

            try
            {
                var result = await _areaRepository.UpdateAsync(areaEntity);
                return result == 1;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while updating {nameof(Area)} in database.", e);
            }
        }
    }
}
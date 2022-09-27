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
    internal class LayoutService : ILayoutService
    {
        private readonly ILayoutRepository _layoutRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<LayoutDto> _layoutValidator;
        public LayoutService(ILayoutRepository layoutRepository,
            IValidator<LayoutDto> layoutValidator,
            IMapper mapper)
        {
            _layoutRepository = layoutRepository;
            _mapper = mapper;
            _layoutValidator = layoutValidator;
        }

        public async Task<LayoutDto> GetByIdAsync(int id)
        {
            var result = await _layoutRepository.GetByIdAsync(id);

            return _mapper.Map<LayoutDto>(result);
        }

        public async Task<IEnumerable<LayoutDto>> GetLayoutsByVenue(int venueId)
        {
            var result = await _layoutRepository.GetLayoutsByVenueIdAsync(venueId);

            return result.Select(l => _mapper.Map<LayoutDto>(l));
        }

        public async Task<bool> AddAsync(LayoutDto layout)
        {
            await _layoutValidator.ValidateAndThrowAsync(layout);

            var layoutEntity = _mapper.Map<Layout>(layout);

            if (await _layoutRepository.IsExists(layoutEntity))
            {
                throw new ServiceException(
                    $"Layout with the same params already exists in DB. Check Layout Description - {layoutEntity.Description} must be unique in Venue.");
            }

            try
            {
                var id = await _layoutRepository.AddAsync(layoutEntity);
                return id > 0;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while adding {nameof(Layout)} in database.", e);
            }
        }

        public async Task<bool> RemoveAsync(int layoutId)
        {
            try
            {
                var result = await _layoutRepository.RemoveAsync(layoutId);
                return result == 1;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while removing {nameof(Layout)} from database.", e);
            }
        }

        public async Task<bool> UpdateAsync(LayoutDto layout)
        {
            await _layoutValidator.ValidateAndThrowAsync(layout);

            var layoutEntity = _mapper.Map<Layout>(layout);

            if (await _layoutRepository.IsExists(layoutEntity))
            {
                throw new ServiceException(
                    $"Layout with the same params already exists in DB. Check Layout Description - {layoutEntity.Description} must be unique in Venue.");
            }

            try
            {
                var result = await _layoutRepository.UpdateAsync(layoutEntity);
                return result == 1;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while updating {nameof(Layout)} in database.", e);
            }
        }
    }
}
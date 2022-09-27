using System.Data.Common;
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
    internal class VenueService : IVenueService
    {
        private readonly IVenueRepository _venueRepository;
        private readonly IValidator<VenueDto> _venueValidator;
        private readonly IMapper _mapper;

        public VenueService(IVenueRepository repository,
            IValidator<VenueDto> venueValidator,
            IMapper mapper)
        {
            _venueRepository = repository;
            _venueValidator = venueValidator;
            _mapper = mapper;
        }

        public async Task<VenueDto> GetByIdAsync(int id)
        {
            var result = await _venueRepository.GetByIdAsync(id);

            return _mapper.Map<VenueDto>(result);
        }

        public async Task<bool> AddAsync(VenueDto venue)
        {
            await _venueValidator.ValidateAndThrowAsync(venue);

            var venueEntity = _mapper.Map<Venue>(venue);

            if (await _venueRepository.IsExists(venueEntity))
            {
                throw new ServiceException(
                    $"Venue with the same params: Description - {venueEntity.Description} already exists in DB.");
            }

            try
            {
                var id = await _venueRepository.AddAsync(venueEntity);
                return id > 0;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while adding {nameof(Venue)} in database.", e);
            }
        }

        public async Task<bool> RemoveAsync(int venueId)
        {
            try
            {
                var result = await _venueRepository.RemoveAsync(venueId);
                return result == 1;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while removing {nameof(Venue)} from database.", e);
            }
        }

        public async Task<bool> UpdateAsync(VenueDto venue)
        {
            await _venueValidator.ValidateAndThrowAsync(venue);

            var venueEntity = _mapper.Map<Venue>(venue);

            if (await _venueRepository.IsExists(venueEntity))
            {
                throw new ServiceException(
                    $"Venue with the same params: Description - {venueEntity.Description} already exists in DB.");
            }

            try
            {
                var result = await _venueRepository.UpdateAsync(venueEntity);
                return result == 1;
            }
            catch (DbException e)
            {
                throw new ServiceException($"Error occurs while updating {nameof(Venue)} in database.", e);
            }
        }
    }
}

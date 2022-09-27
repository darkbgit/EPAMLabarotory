using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TicketManagement.Core.EventManagement.Services.Interfaces;
using TicketManagement.Core.Public.DTOs.VenueDTOs;
using TicketManagement.Core.Public.Exceptions;
using TicketManagement.Core.Public.Localization;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.Public;

namespace TicketManagement.Core.EventManagement.Services;

internal sealed class VenueService : IVenueService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SharedResource> _serviceResourceStringLocalizer;

    public VenueService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizer<SharedResource> serviceResourceStringLocalizer)
    {
        _mapper = mapper;
        _serviceResourceStringLocalizer = serviceResourceStringLocalizer;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<VenueDto>> GetAllVenuesAsync(CancellationToken cancellationToken = default)
    {
        var venues = await _unitOfWork.Venue
            .FindAll()
            .OrderBy(venue => venue.Description)
            .ToListAsync(cancellationToken);

        var venuesDto = venues
            .Select(venue => _mapper.Map<VenueDto>(venue))
            .ToList();
        return venuesDto;
    }

    public async Task<VenueDto> GetByIdAsync(int venueId, CancellationToken cancellationToken = default)
    {
        var venue = await _unitOfWork.Venue.GetByIdAsync(venueId, cancellationToken);
        if (venue is null)
        {
            throw new ServiceException(_serviceResourceStringLocalizer["error.NotFound", "Venue", venueId]);
        }

        var venueDto = _mapper.Map<VenueDto>(venue);
        return venueDto;
    }

    public async Task<VenueDto> CreateAsync(VenueForCreateDto venueForCreateDto, CancellationToken cancellationToken = default)
    {
        var venue = _mapper.Map<Venue>(venueForCreateDto);
        _unitOfWork.Venue.CreateVenue(venue);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<VenueDto>(venue);
    }

    public async Task UpdateAsync(VenueForUpdateDto venueForUpdateDto, CancellationToken cancellationToken = default)
    {
        var venue = await _unitOfWork.Venue.GetByIdAsync(venueForUpdateDto.Id, cancellationToken);

        if (venue is null)
        {
            throw new ServiceException(_serviceResourceStringLocalizer["error.NotFound", venueForUpdateDto.Id]);
        }

        _mapper.Map(venueForUpdateDto, venue);
        _unitOfWork.Venue.UpdateVenue(venue);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(int venueId, CancellationToken cancellationToken = default)
    {
        var venue = await _unitOfWork.Venue.GetByIdAsync(venueId, cancellationToken);

        if (venue is null)
        {
            throw new ServiceException(_serviceResourceStringLocalizer["error.NotFound", venueId]);
        }

        _unitOfWork.Venue.DeleteVenue(venue);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
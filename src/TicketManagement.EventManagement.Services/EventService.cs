using System.Data.Common;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TicketManagement.Core.EventManagement.Services.Interfaces;
using TicketManagement.Core.EventManagement.Services.Validation;
using TicketManagement.Core.Public.DTOs.EventDTOs;
using TicketManagement.Core.Public.Enums;
using TicketManagement.Core.Public.Exceptions;
using TicketManagement.Core.Public.Localization;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.Public;

namespace TicketManagement.Core.EventManagement.Services
{
#pragma warning disable S1200
    internal sealed class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly EventServiceValidator _eventServiceValidator;
        private readonly IStringLocalizer<SharedResource> _serviceResourcesStringLocalizer;
        private readonly IStringLocalizer<EventService> _stringLocalizer;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IImageBase64Service _imageService;

        public EventService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IStringLocalizer<SharedResource> serviceResourcesStringLocalizer,
            IStringLocalizer<EventService> stringLocalizer,
            IWebHostEnvironment hostingEnvironment,
            IImageBase64Service imageService,
            EventServiceValidator eventServiceValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _serviceResourcesStringLocalizer = serviceResourcesStringLocalizer;
            _stringLocalizer = stringLocalizer;
            _hostingEnvironment = hostingEnvironment;
            _imageService = imageService;
            _eventServiceValidator = eventServiceValidator;
        }

        public async Task<PaginatedList<EventForListDto>> GetPagedUpcomingEventsAsync(int page, int eventsPerPage, CancellationToken cancellationToken = default)
        {
            var items = await _unitOfWork.Event.GetEventsForMainPage()
                .Skip((page - 1) * eventsPerPage)
                .Take(eventsPerPage)
                .ToListAsync(cancellationToken);

            var count = await _unitOfWork.Event.EventsForManePageCountAsync(cancellationToken);

            var result = new PaginatedList<EventForListDto>(items, count, page, eventsPerPage);

            return result;
        }

        public async Task<PaginatedList<EventForEditListDto>> GetPagedEventsAsync(
            string sortOrder,
            string searchString,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.Event.GetEventsForEditList();

            var isNeedSearch = !string.IsNullOrEmpty(searchString);

            if (isNeedSearch)
            {
                query = query.Where(eventForModeratorListDto => eventForModeratorListDto.Name.Contains(searchString) ||
                                                   eventForModeratorListDto.VenueDescription.Contains(searchString) ||
                                                   eventForModeratorListDto.LayoutDescription.Contains(searchString));
            }

            if (!Enum.TryParse(sortOrder, true, out ModeratorPanelEventListSortOrder sortOrderEnum))
            {
                sortOrderEnum = ModeratorPanelEventListSortOrder.Name;
            }

            query = ApplySorting(query, sortOrderEnum);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var count = await _unitOfWork.Event.EventsForEditListCountAsync(searchString, cancellationToken);

            var result = new PaginatedList<EventForEditListDto>(items,
            count,
            pageNumber,
            pageSize);

            return result;
        }

        public async Task<EventWithVenueIdDto?> GetEventWithVenueIdByIdAsync(int eventId, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.Event.GetEventsWithVenueIdById()
                .Where(e => e.Id == eventId)
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }

        public async Task<EventWithLayoutsDto?> GetEventWithLayoutsByIdAsync(int eventId, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.Event.GetEventsWithLayoutsAndVenueTimeZoneById()
                .Where(e => e.Id == eventId)
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }

        public async Task<EventDto?> GetByIdAsync(int eventId, CancellationToken cancellationToken = default)
        {
            var @event = await _unitOfWork.Event.GetByIdAsync(eventId, cancellationToken);

            var result = _mapper.Map<EventDto>(@event);

            return result;
        }

        public async Task<EventForDetailsDto?> GetEventWithDetailsByIdAsync(int eventId, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.Event.GetEventsWithDetailsById()
                .Where(@event => @event.Id == eventId)
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }

        public async Task<EventInfoForEventAreaListDto?> GetEventForEventAreaListAsync(int eventId, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.Event.GetEventsForEventAreaList()
                .Where(@event => @event.Id == eventId)
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }

        public async Task<int> CreateAsync(EventForCreateDto eventForCreateDto,
            CancellationToken cancellationToken = default)
        {
            var eventEntity = _mapper.Map<Event>(eventForCreateDto);

            try
            {
                var eventId = await AddAsync(eventEntity, cancellationToken);

                return eventId;
            }
            catch (Exception e)
            {
                throw new ServiceException(e.Message, e);
            }
        }

        public async Task<int> CreateAsync(ThirdPartyEventDto thirdPartyEventDto, CancellationToken cancellationToken = default)
        {
            var imagePath = string.Empty;

            if (!string.IsNullOrEmpty(thirdPartyEventDto.PosterImage))
            {
                imagePath = await _imageService.SaveImgFromBase64(thirdPartyEventDto.PosterImage);
            }

            var eventEntity = _mapper.Map<Event>(thirdPartyEventDto);

            eventEntity.ImageUrl = imagePath;

            try
            {
                var eventId = await AddAsync(eventEntity, cancellationToken);

                return eventId;
            }
            catch (Exception e)
            {
                throw new ServiceException(e.Message, e);
            }
        }

        public async Task UpdateAsync(EventForUpdateDto eventDto, CancellationToken cancellationToken = default)
        {
            var @event = await _unitOfWork.Event.GetByIdAsync(eventDto.Id, cancellationToken);

            if (@event is null)
            {
                throw new ServiceException(_serviceResourcesStringLocalizer["error.Update", nameof(Event)]);
            }

            var isLayoutUpdated = eventDto.LayoutId != @event.LayoutId;

            _mapper.Map(eventDto, @event);

            await _eventServiceValidator.EventValidator.ValidateAndThrowAsync(@event, cancellationToken);

            var isExist = await _unitOfWork.Event.IsExistForUpdateAsync(@event, cancellationToken);

            if (isExist)
            {
                throw new ServiceException(_stringLocalizer["error.IsExist", @event.Description, @event.Name, @event.StartDate, @event.EndDate]);
            }

            if (isLayoutUpdated)
            {
                var eventAreas = _unitOfWork.EventArea.GetEventAreasByEventId(@event.Id);

                var isEventAreasHaveAnyOccupiedSeat = await IsEventAreasHaveAnyOccupiedSeat(eventAreas);

                if (isEventAreasHaveAnyOccupiedSeat)
                {
                    throw new ServiceException(_stringLocalizer["error.EventAreaHaveOccupiedSeat"]);
                }

                foreach (var eventArea in eventAreas)
                {
                    _unitOfWork.EventArea.DeleteEventArea(eventArea);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }

                await CreateEventAreasWithEventSeatsByLayoutId(@event.LayoutId, @event.Id, cancellationToken);
            }

            try
            {
                _unitOfWork.Event.UpdateEvent(@event);
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new ServiceException(_serviceResourcesStringLocalizer["error.Update", nameof(Event)], e);
            }
        }

        public async Task DeleteAsync(int eventId, CancellationToken cancellationToken = default)
        {
            var @event = await _unitOfWork.Event.GetByIdAsync(eventId, cancellationToken);

            if (@event is null)
            {
                throw new ServiceException(_serviceResourcesStringLocalizer["error.Delete", nameof(Event)]);
            }

            var eventAreas = await _unitOfWork.EventArea
                .GetEventAreasByEventId(@event.Id)
                .ToListAsync(cancellationToken);

            var isEventAreasHaveAnyOccupiedSeat = await IsEventAreasHaveAnyOccupiedSeat(eventAreas);

            if (isEventAreasHaveAnyOccupiedSeat)
            {
                throw new ServiceException(_stringLocalizer["error.EventAreaHaveOccupiedSeat"]);
            }

            try
            {
                _unitOfWork.Event.DeleteEvent(eventId);
                if (!@event.ImageUrl.StartsWith("http"))
                {
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, @event.ImageUrl);

                    File.Delete(filePath);
                }
            }
            catch (Exception e)
            {
                throw new ServiceException(_serviceResourcesStringLocalizer["error.Delete", nameof(Event)], e);
            }
        }

#pragma warning disable S1541
        private static IQueryable<EventForEditListDto> ApplySorting(IQueryable<EventForEditListDto> query, ModeratorPanelEventListSortOrder sortOrder)
        {
            query = sortOrder switch
            {
                ModeratorPanelEventListSortOrder.VenueNameDesc => query.OrderByDescending(q => q.VenueDescription),
                ModeratorPanelEventListSortOrder.VenueName => query.OrderBy(q => q.VenueDescription),
                ModeratorPanelEventListSortOrder.LayoutNameDesc => query.OrderByDescending(q => q.LayoutDescription),
                ModeratorPanelEventListSortOrder.LayoutName => query.OrderBy(q => q.LayoutDescription),
                ModeratorPanelEventListSortOrder.StartDateDesc => query.OrderByDescending(q => q.StartDate),
                ModeratorPanelEventListSortOrder.StartDate => query.OrderBy(q => q.StartDate),
                ModeratorPanelEventListSortOrder.DurationDesc => query.OrderByDescending(q => q.Duration),
                ModeratorPanelEventListSortOrder.Duration => query.OrderBy(q => q.Duration),
                ModeratorPanelEventListSortOrder.NameDesc => query.OrderByDescending(c => c.Name),
                ModeratorPanelEventListSortOrder.Name => query.OrderBy(c => c.Name),
                _ => query.OrderBy(c => c.Name)
            };

            return query;
        }

#pragma warning restore S1541

        private async Task<int> AddAsync(Event eventEntity, CancellationToken cancellationToken)
        {
            await _eventServiceValidator.EventValidator.ValidateAndThrowAsync(eventEntity, cancellationToken);

            if (await _unitOfWork.Layout.GetByIdAsync(eventEntity.LayoutId, cancellationToken) == null)
            {
                throw new ServiceException(_serviceResourcesStringLocalizer["error.AddNotValidLayout", eventEntity.LayoutId]);
            }

            var isExist = await _unitOfWork.Event.IsExistAsync(eventEntity, cancellationToken);

            if (isExist)
            {
                throw new ServiceException(_stringLocalizer["error.IsExist", eventEntity.Description, eventEntity.Name, eventEntity.StartDate, eventEntity.EndDate]);
            }

            var areas = await _unitOfWork.Area.GetAreasByLayoutId(eventEntity.LayoutId).ToListAsync(cancellationToken);

            var isAreaHaveAnySeat = await IsAreasHaveAnySeat(areas);

            if (!isAreaHaveAnySeat)
            {
                throw new ServiceException(_stringLocalizer["error.AnySeat"]);
            }

            int eventId;

            try
            {
                eventId = _unitOfWork.Event.CreateEvent(eventEntity);
            }
            catch (DbException e)
            {
                throw new ServiceException(_serviceResourcesStringLocalizer["error.Add", nameof(Event)], e);
            }

            if (eventId <= 0)
            {
                throw new ServiceException(_serviceResourcesStringLocalizer["error.Add", nameof(Event)]);
            }

            try
            {
                await CreateEventAreasWithEventSeatsByLayoutId(eventEntity.LayoutId, eventId, cancellationToken);

                return eventId;
            }
            catch (Exception e)
            {
                _unitOfWork.Event.DeleteEvent(eventId);
                throw new ServiceException(_serviceResourcesStringLocalizer["error.Add", nameof(Event)], e);
            }
        }

        private async Task<bool> IsAreasHaveAnySeat(IEnumerable<Area> areas)
        {
            foreach (var area in areas)
            {
                if (await _unitOfWork.Seat.GetSeatsByAreaId(area.Id).AnyAsync())
                {
                    return true;
                }
            }

            return false;
        }

        private async Task<bool> IsEventAreasHaveAnyOccupiedSeat(IEnumerable<EventArea> eventAreas)
        {
            foreach (var eventArea in eventAreas)
            {
                if (await _unitOfWork.EventSeat
                    .GetEventSeatsByEventAreaId(eventArea.Id)
                    .AnyAsync(eventSeat => eventSeat.State != (int)SeatState.Free))
                {
                    return true;
                }
            }

            return false;
        }

        private async Task CreateEventAreasWithEventSeatsByLayoutId(int layoutId, int eventId, CancellationToken cancellationToken)
        {
            var areas = await _unitOfWork.Area.GetAreasByLayoutId(layoutId).ToListAsync(cancellationToken);

            if (!await IsAreasHaveAnySeat(areas))
            {
                throw new ServiceException(_stringLocalizer["error.AnySeat"]);
            }

            foreach (var area in areas)
            {
                var eventAreaEntity = _mapper.Map<EventArea>(area);
                eventAreaEntity.EventId = eventId;

                await _eventServiceValidator.EventAreaValidator.ValidateAndThrowAsync(eventAreaEntity, cancellationToken);

                if (await _unitOfWork.EventArea.IsExistAsync(eventAreaEntity, cancellationToken))
                {
                    throw new ServiceException(_stringLocalizer["error.IsEventAreaExist", eventAreaEntity.Description, eventAreaEntity.CoordX, eventAreaEntity.CoordY]);
                }

                _unitOfWork.EventArea.CreateEventArea(eventAreaEntity);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var seats = await _unitOfWork.Seat
                    .GetSeatsByAreaId(area.Id)
                    .ToListAsync(cancellationToken);

                var eventSeats = seats.Select(seat =>
                {
                    var eventSeat = _mapper.Map<EventSeat>(seat);
                    eventSeat.State = (int)SeatState.Free;
                    eventSeat.EventAreaId = eventAreaEntity.Id;
                    return eventSeat;
                })
                    .ToList();

                await _eventServiceValidator.EventSeatListValidator.ValidateAndThrowAsync(eventSeats, cancellationToken);

                _unitOfWork.EventSeat.AddEventSeatsRange(eventSeats);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
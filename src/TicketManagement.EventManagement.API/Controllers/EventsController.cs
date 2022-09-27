using Microsoft.AspNetCore.Mvc;
using TicketManagement.Core.EventManagement.Services.Interfaces;
using TicketManagement.Core.Public.DTOs.EventDTOs;
using TicketManagement.Core.Public.Enums;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.Core.Public.Requests;
using AllowAnonymousAttribute = TicketManagement.Core.Public.Filters.AllowAnonymousAttribute;
using AuthorizeAttribute = TicketManagement.Core.Public.Filters.AuthorizeAttribute;

namespace TicketManagement.EventManagement.API.Controllers
{
    [Route("api/event-management/[controller]")]
    [ApiController]
    [Authorize(Roles.Moderator)]
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Get Event by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEventById(int id)
        {
            var @event = await _eventService.GetByIdAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        /// <summary>
        /// Get paginated events for moderator list.
        /// </summary>
        [HttpGet("edit-list")]
        public async Task<ActionResult<PaginatedList<EventForEditListDto>>> GetEventsForEditList([FromQuery] PaginationSearchSortRequest request)
        {
            const int defaultPage = 1;

            var events = await _eventService.GetPagedEventsAsync(request.SortOrder ?? string.Empty, request.SearchString ?? string.Empty,
                request.PageIndex ?? defaultPage, request.PageSize);

            return events;
        }

        /// <summary>
        /// Get paginated events for main page.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("main-page")]
        public async Task<ActionResult<PaginatedList<EventForListDto>>> GetEventsForMainPage([FromQuery] PaginationRequest request)
        {
            const int defaultPage = 1;

            var events = await _eventService.GetPagedUpcomingEventsAsync(request.PageIndex ?? defaultPage, request.PageSize);

            return events;
        }

        /// <summary>
        /// Get event  by id with layouts.
        /// </summary>
        [HttpGet("with-layouts/{id}")]
        public async Task<ActionResult<EventWithLayoutsDto>> GetEventWithLayouts(int id)
        {
            var eventDto = await _eventService.GetEventWithLayoutsByIdAsync(id);

            if (eventDto == null)
            {
                return NotFound();
            }

            return eventDto;
        }

        /// <summary>
        /// Get event name and start date by id  with layout name.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("with-layout-name/{id}")]
        public async Task<ActionResult<EventInfoForEventAreaListDto>> GetEventWithLayoutName(int id)
        {
            var eventDto = await _eventService.GetEventForEventAreaListAsync(id);

            if (eventDto == null)
            {
                return NotFound();
            }

            return eventDto;
        }

        /// <summary>
        /// Get event by id with venue and layout info.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("with-details/{id}")]
        public async Task<ActionResult<EventForDetailsDto>> GetEventWithDetails(int id)
        {
            var eventDto = await _eventService.GetEventWithDetailsByIdAsync(id);

            if (eventDto == null)
            {
                return NotFound();
            }

            return eventDto;
        }

        /// <summary>
        /// Create event.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<EventDto>> AddEvent([FromBody] EventForCreateDto dto)
        {
            var id = await _eventService.CreateAsync(dto);
            var @event = await _eventService.GetByIdAsync(id);

            return CreatedAtAction(nameof(GetEventById), new { id }, @event);
        }

        /// <summary>
        /// Create event from third party editor.
        /// </summary>
        [HttpPost("third-party-editor")]
        public async Task<ActionResult<EventDto>> AddEventFromThirdParty([FromBody] ThirdPartyEventDto dto)
        {
            var id = await _eventService.CreateAsync(dto);
            var @event = await _eventService.GetByIdAsync(id);

            return CreatedAtAction(nameof(GetEventById), new { id }, @event);
        }

        /// <summary>
        /// Update Event by id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventForUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var @event = await _eventService.GetByIdAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            await _eventService.UpdateAsync(dto);

            return NoContent();
        }

        /// <summary>
        /// Delete Event by id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var @event = await _eventService.GetByIdAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            await _eventService.DeleteAsync(id);

            return NoContent();
        }
    }
}

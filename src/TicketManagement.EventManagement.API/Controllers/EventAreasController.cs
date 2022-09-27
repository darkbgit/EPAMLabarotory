using Microsoft.AspNetCore.Mvc;
using TicketManagement.Core.EventManagement.Services.Interfaces;
using TicketManagement.Core.Public.DTOs.EventAreaDTOs;
using TicketManagement.Core.Public.Enums;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.Core.Public.Requests;
using AllowAnonymousAttribute = TicketManagement.Core.Public.Filters.AllowAnonymousAttribute;
using AuthorizeAttribute = TicketManagement.Core.Public.Filters.AuthorizeAttribute;

namespace TicketManagement.EventManagement.API.Controllers
{
    [Route("api/event-management/event-areas")]
    [ApiController]
    [Authorize(Roles.Moderator)]
    public class EventAreasController : ControllerBase
    {
        private readonly IEventAreaService _eventAreaService;

        public EventAreasController(IEventAreaService eventAreaService)
        {
            _eventAreaService = eventAreaService;
        }

        /// <summary>
        /// Get paginated event areas by event id with number of seats number in each area.
        /// </summary>
        [Route("~/api/event-management/events/{id}/event-areas/paginated-with-seat")]
        [HttpGet]
        public async Task<ActionResult<PaginatedList<EventAreaWithSeatsNumberDto>>> GetEventAreasForEditList([FromRoute] int id, [FromQuery] PaginationRequest request)
        {
            const int defaultPage = 1;

            var events = await _eventAreaService.GetPagedEventAreasByEventIdAsync(id, request.PageIndex ?? defaultPage, request.PageSize);

            return Ok(events);
        }

        /// <summary>
        /// Get event areas by event id with number of all seats and free seats in each area.
        /// </summary>
        [AllowAnonymous]
        [Route("~/api/event-management/events/{id}/event-areas/with-seats")]
        [HttpGet]
        public async Task<ActionResult<List<EventAreaWithSeatsAndFreeSeatsCountDto>>> GetEventAreasWithSeatsInfoByEventId([FromRoute] int id)
        {
            var eventAreas = (await _eventAreaService.GetEventAreasWithSeatsInfoByEventIdAsync(id))
                .ToList();

            return Ok(eventAreas);
        }

        /// <summary>
        /// Get event area by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<EventAreaDto>> GetEventAreaById(int id)
        {
            var eventArea = await _eventAreaService.GetByIdAsync(id);

            if (eventArea == null)
            {
                return NotFound();
            }

            return Ok(eventArea);
        }

        /// <summary>
        /// Delete event area by id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventArea(int id)
        {
            var @event = await _eventAreaService.GetByIdAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            await _eventAreaService.DeleteAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Update event area by id.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEventArea(int id, [FromBody] EventAreaDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var @event = await _eventAreaService.GetByIdAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            await _eventAreaService.UpdateAsync(dto);

            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using TicketManagement.Core.EventManagement.Services.Interfaces;
using TicketManagement.Core.Public.DTOs.EventSeatDTOs;
using TicketManagement.Core.Public.Enums;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.Core.Public.Requests;
using AllowAnonymousAttribute = TicketManagement.Core.Public.Filters.AllowAnonymousAttribute;
using AuthorizeAttribute = TicketManagement.Core.Public.Filters.AuthorizeAttribute;

namespace TicketManagement.EventManagement.API.Controllers
{
    [Route("api/event-management/event-seats")]
    [ApiController]
    [Authorize(Roles.Moderator)]
    public class EventSeatsController : ControllerBase
    {
        private readonly IEventSeatService _eventSeatService;

        public EventSeatsController(IEventSeatService eventSeatService)
        {
            _eventSeatService = eventSeatService;
        }

        [Route("~/api/event-management/event-areas/{id}/event-seats/paginated")]
        [HttpGet]
        public async Task<ActionResult<PaginatedList<EventSeatDto>>> GetPaginatedEventSeatsForEditList([FromRoute] int id, [FromQuery] PaginationRequest request)
        {
            const int defaultPage = 1;

            var eventSeats = await _eventSeatService.GetPagedEventSeatsByEventAreaIdAsync(id, request.PageIndex ?? defaultPage, request.PageSize);

            return eventSeats;
        }

        [AllowAnonymous]
        [Route("~/api/event-management/event-areas/{id}/event-seats")]
        [HttpGet]
        public async Task<ActionResult<List<EventSeatForListDto>>> GetAllEventSeatsByEventAreaId([FromRoute] int id)
        {
            var eventSeats = (await _eventSeatService.GetAllEventSeatsByEventAreaIdAsync(id))
                .ToList();

            return eventSeats;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventSeatDto>> GetEventSeatById(int id)
        {
            var eventArea = await _eventSeatService.GetByIdAsync(id);

            if (eventArea == null)
            {
                return NotFound();
            }

            return eventArea;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventSeat(int id)
        {
            var @event = await _eventSeatService.GetByIdAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            await _eventSeatService.DeleteAsync(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEventSeat(int id, [FromBody] EventSeatDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var @event = await _eventSeatService.GetByIdAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            await _eventSeatService.UpdateAsync(dto);

            return NoContent();
        }
    }
}

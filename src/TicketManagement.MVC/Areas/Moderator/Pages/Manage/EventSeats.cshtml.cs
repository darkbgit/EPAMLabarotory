using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Refit;
using TicketManagement.Core.Public.DTOs.EventSeatDTOs;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.Core.Public.Requests;
using TicketManagement.MVC.Clients.EventManagement;

namespace TicketManagement.MVC.Areas.Moderator.Pages.Manage
{
    public class EventSeatsModel : PageModel
    {
        private readonly IEventSeatsClient _eventSeatsClient;
        private readonly IStringLocalizer<EventSeatsModel> _stringLocalizer;

        public EventSeatsModel(IEventSeatsClient eventSeatsClient,
            IStringLocalizer<EventSeatsModel> stringLocalizer)
        {
            _eventSeatsClient = eventSeatsClient;
            _stringLocalizer = stringLocalizer;
        }

        [BindProperty]
        public int EventAreaId { get; set; }

        [BindProperty]
        public int EventId { get; set; }

        public PaginatedList<EventSeatDto> EventSeats { get; set; }

        public async Task<IActionResult> OnGetAsync(int eventId, int eventAreaId, int? pageIndex)
        {
            const int defaultPage = 1;

            var query = new PaginationRequest
            {
                PageSize = Utilities.Constants.EventAreasPerEditPage,
                PageIndex = pageIndex ?? defaultPage,
            };

            try
            {
                EventSeats = await _eventSeatsClient.GetPaginatedEventSeatsForEditList(eventAreaId, query);
            }
            catch
            {
                return BadRequest();
            }

            EventAreaId = eventAreaId;

            EventId = eventId;

            return Page();
        }

        public async Task<IActionResult> OnPostDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                await _eventSeatsClient.DeleteEventSeat(id.Value);
                TempData["Result"] = _stringLocalizer["DeleteSuccess"].Value;
                return RedirectToPage("./EventSeats", new { eventId = EventId, eventAreaId = EventAreaId });
            }
            catch (ApiException e)
            {
                var content = await e.GetContentAsAsync<Dictionary<string, string>>();

                var message = _stringLocalizer["UnknownError"].Value;

                if (content != null)
                {
                    message = string.Join(".", content.Values);
                }

                TempData["Result"] = message;
                return RedirectToPage("./EventSeats", new { eventId = EventId, eventAreaId = EventAreaId });
            }
        }
    }
}

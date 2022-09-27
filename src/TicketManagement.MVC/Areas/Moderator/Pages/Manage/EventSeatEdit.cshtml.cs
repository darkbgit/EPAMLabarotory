using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Refit;
using TicketManagement.Core.Public.DTOs.EventSeatDTOs;
using TicketManagement.MVC.Clients.EventManagement;

namespace TicketManagement.MVC.Areas.Moderator.Pages.Manage
{
    public class EventSeatEditModel : PageModel
    {
        private readonly IEventSeatsClient _eventSeatsClient;
        private readonly IStringLocalizer<EventSeatEditModel> _stringLocalizer;
        private readonly ILogger<EventSeatEditModel> _logger;

        public EventSeatEditModel(ILogger<EventSeatEditModel> logger,
            IEventSeatsClient eventSeatsClient, IStringLocalizer<EventSeatEditModel> stringLocalizer)
        {
            _logger = logger;
            _eventSeatsClient = eventSeatsClient;
            _stringLocalizer = stringLocalizer;
        }

        [BindProperty]
        public EventSeatDto EventSeat { get; set; }

        [BindProperty]
        public int EventId { get; set; }

        public async Task<IActionResult> OnGetAsync(int eventId, int id)
        {
            EventId = eventId;

            try
            {
                EventSeat = await _eventSeatsClient.GetEventSeatById(id);
            }
            catch
            {
                return BadRequest();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _eventSeatsClient.UpdateEventSeat(EventSeat.Id, EventSeat);
                TempData["Result"] = _stringLocalizer["UpdateSuccess"].Value;
                return RedirectToPage("./EventSeats", new { eventAreaId = EventSeat.EventAreaId });
            }
            catch (ValidationApiException e)
            {
                var content = await e.GetContentAsAsync<Dictionary<string, string>>();
                if (content != null)
                {
                    foreach (var error in content)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }
                }
            }
            catch (ApiException e)
            {
                _logger.LogError("{e}", e);
                var content = await e.GetContentAsAsync<Dictionary<string, string>>();
                if (content != null)
                {
                    foreach (var error in content)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }
                }
            }

            return Page();
        }
    }
}

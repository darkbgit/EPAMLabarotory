using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Refit;
using TicketManagement.Core.Public.DTOs.EventAreaDTOs;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.Core.Public.Requests;
using TicketManagement.MVC.Clients.EventManagement;

namespace TicketManagement.MVC.Areas.Moderator.Pages.Manage
{
    public class EventAreasModel : PageModel
    {
        private readonly IEventAreasClient _eventAreasClient;
        private readonly IStringLocalizer<EventAreasModel> _stringLocalizer;

        public EventAreasModel(IStringLocalizer<EventAreasModel> stringLocalizer,
            IEventAreasClient eventAreasClient)
        {
            _stringLocalizer = stringLocalizer;
            _eventAreasClient = eventAreasClient;
        }

        [BindProperty]
        public int EventId { get; set; }

        public PaginatedList<EventAreaWithSeatsNumberDto> EventAreas { get; set; }

        public async Task<IActionResult> OnGetAsync(int eventId, int? pageIndex)
        {
            const int defaultPage = 1;

            var query = new PaginationRequest
            {
                PageSize = Utilities.Constants.EventAreasPerEditPage,
                PageIndex = pageIndex ?? defaultPage,
            };

            try
            {
                EventAreas = await _eventAreasClient.GetPaginatedEventAreasForEditList(eventId, query);
            }
            catch
            {
                return BadRequest();
            }

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
                await _eventAreasClient.DeleteEventArea(id.Value);
                TempData["Result"] = _stringLocalizer["DeleteSuccess"].Value;
                return RedirectToPage("./EventAreas", new { eventId = EventId });
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
                return RedirectToPage("./EventAreas", new { eventId = EventId });
            }
        }
    }
}

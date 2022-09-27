using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Refit;
using TicketManagement.Core.Public.DTOs.EventAreaDTOs;
using TicketManagement.MVC.Clients.EventManagement;

namespace TicketManagement.MVC.Areas.Moderator.Pages.Manage
{
    public class EventAreaEditModel : PageModel
    {
        private readonly IEventAreasClient _eventAreaClient;
        private readonly IStringLocalizer<EventAreaEditModel> _stringLocalizer;
        private readonly ILogger<EventAreaEditModel> _logger;

        public EventAreaEditModel(ILogger<EventAreaEditModel> logger,
            IEventAreasClient eventAreaClient,
            IStringLocalizer<EventAreaEditModel> stringLocalizer)
        {
            _logger = logger;
            _eventAreaClient = eventAreaClient;
            _stringLocalizer = stringLocalizer;
        }

        [BindProperty]
        public EventAreaDto EventArea { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                EventArea = await _eventAreaClient.GetEventAreaById(id);
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
                await _eventAreaClient.UpdateEventArea(EventArea.Id, EventArea);
                TempData["Result"] = _stringLocalizer["UpdateSuccess"].Value;
                return RedirectToPage("./EventAreas", new { eventId = EventArea.EventId });
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

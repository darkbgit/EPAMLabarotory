using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Refit;
using TicketManagement.Core.Public.DTOs.EventDTOs;
using TicketManagement.MVC.Clients.EventManagement;
using TicketManagement.MVC.Models.ViewModels.Event;
using TicketManagement.MVC.Utilities;
using TimeZoneNames;

namespace TicketManagement.MVC.Areas.Moderator.Pages.Manage
{
    public class EventEditModel : PageModel
    {
        private readonly ILogger<EventEditModel> _logger;
        private readonly IStringLocalizer<EventEditModel> _stringLocalizer;
        private readonly IMapper _mapper;
        private readonly IEventsClient _eventsClient;
        private readonly ILayoutsClient _layoutsClient;

        public EventEditModel(IMapper mapper,
            ILogger<EventEditModel> logger,
            IStringLocalizer<EventEditModel> stringLocalizer,
            IEventsClient eventsClient, ILayoutsClient layoutsClient)
        {
            _mapper = mapper;
            _logger = logger;
            _stringLocalizer = stringLocalizer;
            _eventsClient = eventsClient;
            _layoutsClient = layoutsClient;
        }

        [BindProperty]
        public string CultureName { get; set; }

        [BindProperty]
        public EditEventViewModel Event { get; set; }

        [BindProperty]
        [Display(Name = "EventTimezone")]
        public string TimeZoneName { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var eventDto = await _eventsClient.GetEventWithLayouts(id);

            if (eventDto == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<EditEventViewModel>(eventDto);

            model.Layouts = new SelectList(eventDto.Layouts.OrderBy(l => l.Description), "Id", "Description");

            Event = model;

            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var cultureName = requestCulture.RequestCulture.Culture.Name == "be-BY" ? "en-US" : requestCulture.RequestCulture.Culture.Name;

            var timeZoneName = TZNames.GetDisplayNameForTimeZone(eventDto.VenueTimeZoneId, cultureName);

            TimeZoneName = timeZoneName;
            CultureName = requestCulture.RequestCulture.Culture.Name;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || !ValidateEvent(Event))
            {
                try
                {
                    var layouts = await _layoutsClient.GetLayoutsByVenueId(Event.VenueId);

                    Event.Layouts = new SelectList(layouts.OrderBy(l => l.Description), "Id", "Description");
                }
                catch
                {
                    return BadRequest();
                }

                return Page();
            }

            var eventDto = _mapper.Map<EventForUpdateDto>(Event);

            try
            {
                await _eventsClient.UpdateEvent(Event.Id, eventDto);
                TempData["Result"] = _stringLocalizer["Update success"].Value;
                return RedirectToPage("./Events");
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

            try
            {
                var layouts = await _layoutsClient.GetLayoutsByVenueId(Event.VenueId);

                Event.Layouts = new SelectList(layouts.OrderBy(l => l.Description), "Id", "Description");
            }
            catch
            {
                return BadRequest();
            }

            return Page();
        }

        private bool ValidateEvent(EditEventViewModel model)
        {
            if (model.StartDate > model.EndDate)
            {
                ModelState.AddModelError("StartDate", _stringLocalizer["error.StartDateLess"]);
                return false;
            }

            if ((model.EndDate - model.StartDate) > TimeSpan.FromHours(Constants.MaxEventLengthHours))
            {
                ModelState.AddModelError("StartDate", _stringLocalizer["error.EventLength", Constants.MaxEventLengthHours]);
                return false;
            }

            return true;
        }
    }
}

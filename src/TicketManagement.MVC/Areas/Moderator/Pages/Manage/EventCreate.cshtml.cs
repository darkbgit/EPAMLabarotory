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
    public class EventCreateModel : PageModel
    {
        private readonly IStringLocalizer<EventCreateModel> _stringLocalizer;
        private readonly IEventsClient _eventsClient;
        private readonly ILayoutsClient _layoutsClient;
        private readonly IVenuesClient _venuesClient;
        private readonly IMapper _mapper;
        private readonly ILogger<EventCreateModel> _logger;

        public EventCreateModel(IStringLocalizer<EventCreateModel> stringLocalizer,
            IMapper mapper,
            ILogger<EventCreateModel> logger,
            IEventsClient eventsClient,
            IVenuesClient venuesClient,
            ILayoutsClient layoutsClient)
        {
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _logger = logger;
            _eventsClient = eventsClient;
            _venuesClient = venuesClient;
            _layoutsClient = layoutsClient;
        }

        [BindProperty]
        public string CultureName { get; set; }

        public SelectList VenueList { get; set; }

        [BindProperty(SupportsGet = true)]
        public int VenueId { get; set; }

        public int LayoutId { get; set; }
        public SelectList TimeZonesList { get; set; }
        public string TimeZoneId { get; set; }

        [BindProperty]
        public CreateEventViewModel Event { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            CultureName = requestCulture.RequestCulture.Culture.TwoLetterISOLanguageName;

            await SetVenuesAndTimeZonesLists();
            return Page();
        }

        public async Task<JsonResult> OnGetLayouts(int venueId)
        {
            var layouts = await _layoutsClient.GetLayoutsByVenueId(venueId);
            return new JsonResult(layouts);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || !ValidateEvent(Event))
            {
                await SetVenuesAndTimeZonesLists();

                return Page();
            }

            try
            {
                var dto = _mapper.Map<EventForCreateDto>(Event);
                await _eventsClient.CreateEvent(dto);
                TempData["Result"] = _stringLocalizer["Create success"].Value;
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

            await SetVenuesAndTimeZonesLists();

            return Page();
        }

        private bool ValidateEvent(CreateEventViewModel model)
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

        private async Task SetVenuesAndTimeZonesLists()
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();
            var cultureName = requestCulture.RequestCulture.Culture.Name == "be-BY" ? "en-US" : requestCulture.RequestCulture.Culture.Name;

            var venues = (await _venuesClient.GetAllVenues())
                .Select(v => new
                {
                    v.Id,
                    v.Description,
                    TimeZoneName = TZNames.GetDisplayNameForTimeZone(v.TimeZoneId, cultureName),
                })
                .ToList();

            VenueList = new SelectList(venues, "Id", "Description");

            TimeZonesList = new SelectList(venues, "Id", "TimeZoneName");
        }
    }
}

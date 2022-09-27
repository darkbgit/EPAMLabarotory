using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Refit;
using TicketManagement.Core.Public.DTOs.EventDTOs;
using TicketManagement.MVC.Clients.EventManagement;
using TicketManagement.MVC.Utilities;
using TicketManagement.MVC.Utilities.Extensions;

namespace TicketManagement.MVC.Areas.Moderator.Pages.Manage
{
    public class EventsLoadModel : PageModel
    {
        private readonly long _fileSizeLimit;
        private readonly string[] _permittedExtensions = { ".json" };
        private readonly IEventsClient _eventsClient;
        private readonly IStringLocalizer<EventsLoadModel> _stringLocalizer;
        private readonly ILogger<EventsLoadModel> _logger;

        public EventsLoadModel(IConfiguration config,
            IStringLocalizer<EventsLoadModel> stringLocalizer,
            ILogger<EventsLoadModel> logger,
            IEventsClient eventsClient)
        {
            _stringLocalizer = stringLocalizer;
            _logger = logger;
            _eventsClient = eventsClient;
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");
        }

        [BindProperty]
        public EventsLoad FileUpload { get; set; }

        public string Result { get; private set; }

        public void OnGet()
        {
            // empty
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Result = _stringLocalizer["badResult"];

                return Page();
            }

            var formFileContent =
                await FileHelpers.ProcessFormFile<EventsLoad>(
                    FileUpload.FormFile, ModelState, _permittedExtensions,
                    _fileSizeLimit);

            if (!ModelState.IsValid)
            {
                Result = _stringLocalizer["badResult"];

                return Page();
            }

            string jsonStr = Encoding.UTF8.GetString(formFileContent);

            List<ThirdPartyEventDto> events;

            try
            {
                events = JsonConvert.DeserializeObject<List<ThirdPartyEventDto>>(jsonStr);
            }
            catch (Exception)
            {
                Result = _stringLocalizer["badResult"];

                return Page();
            }

            if (events == null || !events.Any())
            {
                Result = _stringLocalizer["badResult"];

                return Page();
            }

            var model = new List<EventsLoadResultModel.EventsLoadResult>();

            foreach (var @event in events)
            {
                var eventsLoadResult = new EventsLoadResultModel.EventsLoadResult
                {
                    Event = new EventDto
                    {
                        Description = @event.Description,
                        StartDate = @event.StartDate,
                        EndDate = @event.EndDate,
                        LayoutId = @event.LayoutId,
                        Name = @event.Name,
                    },
                };

                try
                {
                    var eventDto = await _eventsClient.CreateEventFromThirdPartyEditor(@event);

                    eventsLoadResult.Event.ImageUrl = eventDto.ImageUrl;
                    eventsLoadResult.IsAddedToDb = true;
                    _logger.LogInformation($"Event with Id - {eventDto.Id} successfully added to database.");
                }
                catch (ApiException e)
                {
                    _logger.LogError("{e}", e);

                    var content = await e.GetContentAsAsync<Dictionary<string, string>>();
                    var message = _stringLocalizer["UnknownError"].Value;

                    if (content != null)
                    {
                        message = string.Join(".", content.Values);
                    }

                    eventsLoadResult.IsAddedToDb = false;
                    eventsLoadResult.Error = message;
                }

                model.Add(eventsLoadResult);
            }

            TempData.Put("ResultModel", model);

            return RedirectToPage("./EventsLoadResult");
        }
    }

    public class EventsLoad
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }

        [Display(Name = "Note")]
        [StringLength(50, MinimumLength = 0)]
        public string? Note { get; set; }
    }
}

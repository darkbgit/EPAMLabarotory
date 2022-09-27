using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Refit;
using TicketManagement.Core.Public.Requests;
using TicketManagement.MVC.Clients.EventManagement;
using TicketManagement.MVC.Utilities;

namespace TicketManagement.MVC.Controllers
{
    public sealed class EventController : Controller
    {
        private readonly ILogger<EventController> _logger;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<EventController> _stringLocalizer;
        private readonly IEventsClient _eventsClient;

        public EventController(ILogger<EventController> logger,
            IMapper mapper,
            IEventsClient eventsClient,
            IStringLocalizer<EventController> stringLocalizer)
        {
            _logger = logger;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _eventsClient = eventsClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? pageIndex)
        {
            const int defaultPage = 1;

            var request = new PaginationRequest
            {
                PageIndex = pageIndex ?? defaultPage,
                PageSize = Constants.EventsPerMainPage,
            };

            try
            {
                var result = await _eventsClient.GetPaginatedEventsForMainPage(request);

                return View(result);
            }
            catch (ApiException)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            try
            {
                var eventDto = await _eventsClient.GetEventWithDetails(id.Value);

                return View(eventDto);
            }
            catch (ApiException)
            {
                return BadRequest();
            }
        }
    }
}

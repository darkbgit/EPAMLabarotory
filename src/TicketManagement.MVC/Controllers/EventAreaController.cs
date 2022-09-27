using Microsoft.AspNetCore.Mvc;
using Refit;
using TicketManagement.Core.Public.DTOs.EventAreaDTOs;
using TicketManagement.Core.Public.DTOs.EventDTOs;
using TicketManagement.MVC.Clients.EventManagement;
using TicketManagement.MVC.Models.ViewModels.EventArea;

namespace TicketManagement.MVC.Controllers
{
    public sealed class EventAreaController : Controller
    {
        private readonly IEventsClient _eventsClient;
        private readonly IEventAreasClient _eventAreasClient;

        public EventAreaController(IEventsClient eventsClient,
            IEventAreasClient eventAreasClient)
        {
            _eventsClient = eventsClient;
            _eventAreasClient = eventAreasClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int eventId)
        {
            EventInfoForEventAreaListDto eventInfo;
            try
            {
                eventInfo = await _eventsClient.GetEventWithLayoutName(eventId);
            }
            catch (ApiException)
            {
                return BadRequest();
            }

            if (eventInfo == null)
            {
                return NotFound();
            }

            List<EventAreaWithSeatsAndFreeSeatsCountDto> eventAreas;

            try
            {
                eventAreas = await _eventAreasClient.GetAllEventAreasByEventId(eventId);
            }
            catch (ApiException)
            {
                return BadRequest();
            }

            var result = new EventAreaListWithInfoViewModel
            {
                EventAreas = eventAreas,
                EventName = eventInfo.EventName,
                LayoutName = eventInfo.LayoutName,
                StartDate = eventInfo.StartDate,
            };

            return View(result);
        }
    }
}

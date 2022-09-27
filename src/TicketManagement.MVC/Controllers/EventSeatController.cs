using Microsoft.AspNetCore.Mvc;
using TicketManagement.MVC.Clients.EventManagement;

namespace TicketManagement.MVC.Controllers
{
    public class EventSeatController : Controller
    {
        private readonly IEventSeatsClient _eventSeatsClient;

        public EventSeatController(IEventSeatsClient eventSeatsClient)
        {
            _eventSeatsClient = eventSeatsClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int eventAreaId)
        {
            var result = await _eventSeatsClient.GetAllEventSeatsByEventAreaId(eventAreaId);

            return PartialView(result);
        }
    }
}

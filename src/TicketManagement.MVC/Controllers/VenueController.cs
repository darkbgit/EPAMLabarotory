using Microsoft.AspNetCore.Mvc;
using TicketManagement.MVC.Clients.EventManagement;

namespace TicketManagement.MVC.Controllers
{
    public class VenueController : Controller
    {
        private readonly IVenuesClient _venueClient;

        public VenueController(IVenuesClient venueClient)
        {
            _venueClient = venueClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _venueClient.GetAllVenues();

            return View(result);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using TicketManagement.Core.EventManagement.Services.Interfaces;
using TicketManagement.Core.Public.DTOs.VenueDTOs;

namespace TicketManagement.EventManagement.API.Controllers
{
    [Route("api/event-management/[controller]")]
    [ApiController]
    public class VenuesController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenuesController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VenueDto>>> GetAllVenues()
        {
            var venues = (await _venueService.GetAllVenuesAsync())
                .ToList();

            return venues;
        }
    }
}

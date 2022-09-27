using Microsoft.AspNetCore.Mvc;
using TicketManagement.Core.EventManagement.Services.Interfaces;
using TicketManagement.Core.Public.DTOs.LayoutDTOs;

namespace TicketManagement.EventManagement.API.Controllers
{
    [Route("api/event-management/[controller]")]
    [ApiController]
    public class LayoutsController : ControllerBase
    {
        private readonly ILayoutService _layoutService;

        public LayoutsController(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }

        [Route("~/api/event-management/venues/{id}/layouts")]
        [HttpGet]
        public async Task<ActionResult<List<LayoutDto>>> GetLayoutsByVenueId([FromRoute] int id)
        {
            var layouts = (await _layoutService.GetLayoutsByVenueIdAsync(id))
                .ToList();

            return layouts;
        }
    }
}

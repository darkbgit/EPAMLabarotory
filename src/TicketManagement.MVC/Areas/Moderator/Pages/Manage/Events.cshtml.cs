using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Refit;
using TicketManagement.Core.Public.DTOs.EventDTOs;
using TicketManagement.Core.Public.Enums;
using TicketManagement.Core.Public.Models.Pagination;
using TicketManagement.Core.Public.Requests;
using TicketManagement.MVC.Clients.EventManagement;

namespace TicketManagement.MVC.Areas.Moderator.Pages.Manage
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = nameof(Roles.Moderator))]
    public class EventsModel : PageModel
    {
        private readonly IEventsClient _eventsClient;
        private readonly IStringLocalizer<EventsModel> _stringLocalizer;

        public EventsModel(IStringLocalizer<EventsModel> stringLocalizer,
            IEventsClient eventsClient)
        {
            _stringLocalizer = stringLocalizer;
            _eventsClient = eventsClient;
        }

        public PaginatedList<EventForEditListDto> Events { get; set; }

        public string NameSort { get; set; }
        public string VenueSort { get; set; }
        public string LayoutSort { get; set; }
        public string StartDateSort { get; set; }
        public string DurationSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public async Task<IActionResult> OnGetAsync(string sortOrder, string currentFilter, string searchString,
            int? pageIndex)
        {
            const int defaultPage = 1;

            CurrentSort = sortOrder;

            NameSort = string.IsNullOrEmpty(sortOrder)
                ? ModeratorPanelEventListSortOrder.NameDesc.ToString()
                : "";
            VenueSort = sortOrder == ModeratorPanelEventListSortOrder.VenueName.ToString()
                ? ModeratorPanelEventListSortOrder.VenueNameDesc.ToString()
                : ModeratorPanelEventListSortOrder.VenueName.ToString();
            LayoutSort = sortOrder == ModeratorPanelEventListSortOrder.LayoutName.ToString()
                ? ModeratorPanelEventListSortOrder.LayoutNameDesc.ToString()
                : ModeratorPanelEventListSortOrder.LayoutName.ToString();
            StartDateSort = sortOrder == ModeratorPanelEventListSortOrder.StartDate.ToString()
                ? ModeratorPanelEventListSortOrder.StartDateDesc.ToString()
                : ModeratorPanelEventListSortOrder.StartDate.ToString();
            DurationSort = sortOrder == ModeratorPanelEventListSortOrder.Duration.ToString()
                ? ModeratorPanelEventListSortOrder.DurationDesc.ToString()
                : ModeratorPanelEventListSortOrder.Duration.ToString();

            if (!string.IsNullOrEmpty(searchString) || !pageIndex.HasValue)
            {
                pageIndex = defaultPage;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            var query = new PaginationSearchSortRequest
            {
                SortOrder = sortOrder,
                SearchString = searchString,
                PageSize = Utilities.Constants.EventsPerModeratorPage,
                PageIndex = pageIndex,
            };

            try
            {
                Events = await _eventsClient.GetPaginatedEventsForEditList(query);
            }
            catch
            {
                return BadRequest();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                await _eventsClient.DeleteEvent(id.Value);
                TempData["Result"] = _stringLocalizer["DeleteSuccess"].Value;
                return RedirectToPage("./Events");
            }
            catch (ApiException e)
            {
                var content = await e.GetContentAsAsync<Dictionary<string, string>>();

                var message = _stringLocalizer["UnknownError"].Value;

                if (content != null)
                {
                    message = string.Join(".", content.Values);
                }

                TempData["Result"] = message;
                return RedirectToPage("./Events");
            }
        }
    }
}

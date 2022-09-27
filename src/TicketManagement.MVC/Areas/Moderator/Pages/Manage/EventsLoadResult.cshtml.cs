using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketManagement.Core.Public.DTOs.EventDTOs;
using TicketManagement.MVC.Utilities.Extensions;

namespace TicketManagement.MVC.Areas.Moderator.Pages.Manage
{
    public class EventsLoadResultModel : PageModel
    {
        public EventsLoadResultModel()
        {
            // empty
        }

        public List<EventsLoadResult> InputModel { get; set; }

        public void OnGet()
        {
            InputModel = TempData.Get<List<EventsLoadResult>>("ResultModel");
        }

        public class EventsLoadResult
        {
            public EventDto Event { get; set; }

            [Display(Name = "Status")]
            public bool IsAddedToDb { get; set; }

            [Display(Name = "Error")]
            public string Error { get; set; }
        }
    }
}

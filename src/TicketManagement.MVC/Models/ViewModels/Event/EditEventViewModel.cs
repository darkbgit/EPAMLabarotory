using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketManagement.MVC.Models.ViewModels.Event
{
    public class EditEventViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        [MaxLength(120)]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
        public int LayoutId { get; set; }

        [Display(Name = "Layouts")]
        public SelectList? Layouts { get; set; }

        [Display(Name = "StartDate")]
        public DateTime StartDate { get; set; }

        [Display(Name = "EndDate")]
        public DateTime EndDate { get; set; }

        [Display(Name = "ImageUrl")]
        public string? ImageUrl { get; set; }

        public int VenueId { get; set; }
    }
}

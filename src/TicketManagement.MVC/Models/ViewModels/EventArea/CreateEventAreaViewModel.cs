using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TicketManagement.MVC.Models.ViewModels.EventArea
{
    public class CreateEventAreaViewModel
    {
        public int AreaId { get; set; }

        public bool IsChecked { get; set; }

        public string Description { get; set; }

        [Display(Name = "CoordX")]
        public int CoordX { get; set; }

        [Display(Name = "CoordY")]
        public int CoordY { get; set; }

        [Display(Name = "Price")]
        [Precision(18, 0)]
        [Range(0, int.MaxValue, ErrorMessage = "Price must be greater or equal 0.")]
        public decimal Price { get; set; }

        [Display(Name = "SeatsNumber")]
        public int SeatsNumber { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace ThirdPartyEventEditor.Models
{
    public class ThirdPartyEventForUpdate
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        public string PosterImage { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "LayoutId must be positive number.")]
        public int LayoutId { get; set; }

        public string OriginalName { get; set; }

        public DateTime OriginalStartDate { get; set; }

        public DateTime OriginalEndDate { get; set; }

        public string OriginalDescription { get; set; }

        public string OriginalPosterImage { get; set; }

        public int OriginalLayoutId { get; set; }
    }
}
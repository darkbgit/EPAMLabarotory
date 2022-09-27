using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Core.Public.Enums
{
    public enum SeatState
    {
        [Display(Name = "Free", Description = "Free")]
        Free,

        [Display(Name = "Occupied", Description = "Occupied")]
        Occupied,

        [Display(Name = "InProcess", Description = "InProcess")]
        InProcess,
    }
}
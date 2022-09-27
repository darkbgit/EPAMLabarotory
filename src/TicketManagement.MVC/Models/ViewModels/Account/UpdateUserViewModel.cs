using System.ComponentModel.DataAnnotations;

namespace TicketManagement.MVC.Models.ViewModels.Account;

public class UpdateUserViewModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; }

    [Required(ErrorMessage = "Введите год рождения")]
    [Range(1980, 2100, ErrorMessage = "Недопустимый год рождения")]
    public int Year { get; set; }

    public string PhoneNumber { get; set; }

    public double MinimalRating { get; set; }
}
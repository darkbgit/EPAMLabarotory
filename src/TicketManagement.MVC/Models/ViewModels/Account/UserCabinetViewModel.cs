using System.ComponentModel.DataAnnotations;

namespace TicketManagement.MVC.Models.ViewModels.Account;

public class UserCabinetViewModel
{
    public Guid Id { get; set; }

    [Display(Name = "Имя пользователя")]
    public string UserName { get; set; }

    public string Email { get; set; }

    [Display(Name = "Год рождения")]
    public int Year { get; set; }

    [Display(Name = "Мобильный телефон")]
    public string PhoneNumber { get; set; }

    [Display(Name = "Минимальный рейтинг новости")]
    public double MinimalRating { get; set; }
}
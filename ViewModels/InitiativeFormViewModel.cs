
using System.ComponentModel.DataAnnotations;

namespace YouthHub.ViewModels;

public class InitiativeFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Моля, въведи заглавие.")]
    [StringLength(100, MinimumLength = 5,
        ErrorMessage = "Заглавието трябва да е между 5 и 100 символа.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Моля, въведи описание.")]
    [StringLength(2000, MinimumLength = 20,
        ErrorMessage = "Описанието трябва да е между 20 и 2000 символа.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Моля, въведи място.")]
    [StringLength(100,
        ErrorMessage = "Мястото не може да е повече от 100 символа.")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Моля, избери дата.")]
    public DateTime EventDate { get; set; }

    [StringLength(80,
        ErrorMessage = "Името на организатора не може да е повече от 80 символа.")]
    public string? Organizer { get; set; }

    public int? CategoryId { get; set; }
}

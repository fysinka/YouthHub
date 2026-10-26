
using System.ComponentModel.DataAnnotations;

namespace YouthHub.ViewModels;

public class ApplicationFormViewModel
{
    [Required(ErrorMessage = "Моля, въведи име.")]
    [StringLength(100, MinimumLength = 2,
        ErrorMessage = "Името трябва да е между 2 и 100 символа.")]
    public string ApplicantName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Моля, въведи имейл.")]
    [EmailAddress(ErrorMessage = "Въведи валиден имейл адрес.")]
    public string Email { get; set; } = string.Empty;

    [Required]
    public int InitiativeId { get; set; }
}


using System.ComponentModel.DataAnnotations;

namespace YouthHub.Models;

public class Application
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Моля, въведи име.")]
    [StringLength(100, MinimumLength = 2)]
    public string ApplicantName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Моля, въведи имейл.")]
    [EmailAddress(ErrorMessage = "Въведи валиден имейл адрес.")]
    public string Email { get; set; } = string.Empty;

    public DateTime AppliedOn { get; set; } = DateTime.UtcNow;

    [Required]
    public int InitiativeId { get; set; }

    public Initiative? Initiative { get; set; }
}


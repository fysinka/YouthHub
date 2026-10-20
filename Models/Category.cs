
using System.ComponentModel.DataAnnotations;

namespace YouthHub.Models;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Моля, въведи име на категорията.")]
    [StringLength(50, MinimumLength = 3,
        ErrorMessage = "Името трябва да е между 3 и 50 символа.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(300,
        ErrorMessage = "Описанието не може да надвишава 300 символа.")]
    public string? Description { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace Comasy.Web.Models;

public class CreatePageViewModel
{
    [Required(ErrorMessage = "Ange en titel")]
    [StringLength(120, MinimumLength = 2,
        ErrorMessage = "Titeln måste vara mellan 2 och 120 tecken")]
    [Display(Name = "Sidans titel")]
    public string Title { get; set; } = string.Empty;
}
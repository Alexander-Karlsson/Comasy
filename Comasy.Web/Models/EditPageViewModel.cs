using System.ComponentModel.DataAnnotations;

namespace Comasy.Web.Models;

public class EditPageViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ange en titel")]
    [StringLength(120, MinimumLength = 2,
        ErrorMessage = "Titeln måste vara mellan 2 och 120 tecken")]
    [Display(Name = "Sidans titel")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Webbadress")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Publicerad")]
    public bool IsPublished { get; set; }
}
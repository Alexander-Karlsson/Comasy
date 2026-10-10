using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Comasy.Web.Models;

public class MenuItemFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ange en text")]
    [StringLength(60, MinimumLength = 1,
        ErrorMessage = "Texten får vara högst 60 tecken")]
    [Display(Name = "Text i menyn")]
    public string Text { get; set; } = string.Empty;

    [Required(ErrorMessage = "Välj vilken sida menyalternativet ska leda till")]
    [Display(Name = "Leder till sida")]
    public int PageId { get; set; }

    [Display(Name = "Undermeny till")]
    public int? ParentId { get; set; }

    [Display(Name = "Synlig i menyn")]
    public bool IsVisible { get; set; } = true;

    // Fylls av controllern, postas aldrig tillbaka.
    public List<SelectListItem> AvailablePages { get; set; } = [];
    public List<SelectListItem> AvailableParents { get; set; } = [];
}
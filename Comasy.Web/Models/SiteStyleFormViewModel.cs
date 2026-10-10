using System.ComponentModel.DataAnnotations;
using Comasy.Core.Enums;

namespace Comasy.Web.Models;

public class SiteStyleFormViewModel
{
    public StyleElement Element { get; set; }

    public string ElementName { get; set; } = string.Empty;

    [Display(Name = "Typsnitt")]
    [RegularExpression(@"^[a-zA-Z0-9 ,'\-]{1,100}$",
        ErrorMessage = "Ange ett typsnittsnamn, t.ex. Georgia, serif")]
    public string? FontFamily { get; set; }

    [Display(Name = "Textstorlek")]
    [RegularExpression(@"^\d{1,4}(\.\d{1,2})?(px|rem|em|%|pt)$",
        ErrorMessage = "Ange storlek med enhet, t.ex. 18px eller 1.5rem")]
    public string? FontSize { get; set; }

    [Display(Name = "Färg")]
    [RegularExpression(@"^(#[0-9a-fA-F]{3,8}|rgb\(\s*\d{1,3}\s*,\s*\d{1,3}\s*,\s*\d{1,3}\s*\)|[a-zA-Z]{3,20})$",
        ErrorMessage = "Ange en färg, t.ex. #336699 eller teal")]
    public string? Color { get; set; }

    [Display(Name = "Tjocklek")]
    [RegularExpression(@"^[a-zA-Z0-9\-]{1,20}$",
        ErrorMessage = "Ange t.ex. normal, bold eller 600")]
    public string? FontWeight { get; set; }

    [Display(Name = "Understrykning")]
    [RegularExpression(@"^[a-zA-Z0-9\-]{1,20}$",
        ErrorMessage = "Ange underline eller none")]
    public string? TextDecoration { get; set; }

    [Display(Name = "Maxbredd")]
    [RegularExpression(@"^\d{1,4}(\.\d{1,2})?(px|rem|em|%|pt)$",
        ErrorMessage = "Ange bredd med enhet, t.ex. 600px eller 100%")]
    public string? MaxWidth { get; set; }

    [Display(Name = "Rundade hörn")]
    [RegularExpression(@"^\d{1,4}(\.\d{1,2})?(px|rem|em|%|pt)$",
        ErrorMessage = "Ange radie med enhet, t.ex. 8px")]
    public string? BorderRadius { get; set; }
}
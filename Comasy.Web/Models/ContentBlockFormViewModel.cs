using System.ComponentModel.DataAnnotations;
using Comasy.Core.Enums;

namespace Comasy.Web.Models;

public class ContentBlockFormViewModel
{
    public int Id { get; set; }
    public int PageId { get; set; }
    public string PageTitle { get; set; } = string.Empty;

    [Display(Name = "Typ av innehåll")] 
    public BlockType BlockType { get; set; } = BlockType.Text;
    
    [Display(Name = "Text")]
    [StringLength(4000, ErrorMessage = "Texten får vara högst 4000 tecken.")]
    public string? Text { get; set; }

    [Display(Name = "Bildadress")]
    [StringLength(500)]
    public string? ImageUrl { get; set; }

    [Display(Name = "Alternativtext")]
    [StringLength(200)]
    public string? AltText { get; set; }

    [Display(Name = "Länkadress")]
    [StringLength(500)]
    public string? LinkUrl { get; set; }

    [Display(Name = "Länktext")]
    [StringLength(200)]
    public string? LinkText { get; set; }
}
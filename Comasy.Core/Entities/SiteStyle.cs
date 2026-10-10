using Comasy.Core.Enums;

namespace Comasy.Core.Entities;

public class SiteStyle
{
    public int Id { get; set; }

    public StyleElement Element { get; set; }

    // Stil gemensamt för rubriker, texter och länkar
    public string? FontFamily { get; set; }
    public string? FontSize { get; set; }
    public string? Color { get; set; }
    public string? FontWeight { get; set; }

    // Endast länkar
    public string? TextDecoration { get; set; }

    // Bara bilder
    public string? MaxWidth { get; set; }
    public string? BorderRadius { get; set; }
}
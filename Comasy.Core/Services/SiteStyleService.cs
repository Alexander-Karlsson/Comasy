using System.Text;
using System.Text.RegularExpressions;
using Comasy.Core.Entities;
using Comasy.Core.Enums;
using Comasy.Core.Interfaces;

namespace Comasy.Core.Services;

public class SiteStyleService(ISiteStyleRepository styleRepository) : ISiteStyleService
{
    // Vitlistor: bara värden som matchar släpps igenom till CSS:en.
    private static readonly Regex ColorPattern =
        new(@"^(#[0-9a-fA-F]{3,8}|rgb\(\s*\d{1,3}\s*,\s*\d{1,3}\s*,\s*\d{1,3}\s*\)|[a-zA-Z]{3,20})$");

    private static readonly Regex SizePattern =
        new(@"^\d{1,4}(\.\d{1,2})?(px|rem|em|%|pt)$");

    private static readonly Regex FontFamilyPattern =
        new(@"^[a-zA-Z0-9 ,'\-]{1,100}$");

    private static readonly Regex KeywordPattern =
        new(@"^[a-zA-Z0-9\-]{1,20}$");

    public Task<IEnumerable<SiteStyle>> GetAllAsync()
        => styleRepository.GetAllAsync();

    public Task<SiteStyle?> GetByElementAsync(StyleElement element)
        => styleRepository.GetByElementAsync(element);

    public Task SaveAsync(SiteStyle style)
        => styleRepository.UpsertAsync(style);

    public async Task<string> GenerateCssAsync()
    {
        var styles = await styleRepository.GetAllAsync();
        var css = new StringBuilder();

        foreach (var style in styles)
        {
            var rules = new List<string>();

            AddRule(rules, "font-family", style.FontFamily, FontFamilyPattern);
            AddRule(rules, "font-size", style.FontSize, SizePattern);
            AddRule(rules, "color", style.Color, ColorPattern);
            AddRule(rules, "font-weight", style.FontWeight, KeywordPattern);
            AddRule(rules, "text-decoration", style.TextDecoration, KeywordPattern);
            AddRule(rules, "max-width", style.MaxWidth, SizePattern);
            AddRule(rules, "border-radius", style.BorderRadius, SizePattern);

            if (rules.Count == 0) continue;

            css.Append(CssSelector(style.Element))
               .Append(" { ")
               .Append(string.Join(" ", rules))
               .AppendLine(" }");
        }

        return css.ToString();
    }

    private static void AddRule(List<string> rules, string property, string? value, Regex pattern)
    {
        if (string.IsNullOrWhiteSpace(value)) return;

        var trimmed = value.Trim();
        if (!pattern.IsMatch(trimmed)) return;

        rules.Add($"{property}: {trimmed};");
    }

    private static string CssSelector(StyleElement element) => element switch
    {
        StyleElement.Heading => ".cms-heading",
        StyleElement.Text => ".cms-text",
        StyleElement.Image => ".cms-image",
        StyleElement.Link => ".cms-link",
        _ => string.Empty
    };
}
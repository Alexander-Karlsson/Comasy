using Comasy.Core.Entities;
using Comasy.Core.Enums;
using Comasy.Core.Interfaces;
using Comasy.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Comasy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class StylesController(ISiteStyleService styleService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var saved = (await styleService.GetAllAsync())
            .ToDictionary(s => s.Element);

        var model = Enum.GetValues<StyleElement>()
            .Select(element => Map(element, saved.GetValueOrDefault(element)))
            .ToList();

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(StyleElement element)
    {
        var style = await styleService.GetByElementAsync(element);
        return View(Map(element, style));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SiteStyleFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.ElementName = ElementName(model.Element);
            return View(model);
        }

        await styleService.SaveAsync(new SiteStyle
        {
            Element = model.Element,
            FontFamily = model.FontFamily,
            FontSize = model.FontSize,
            Color = model.Color,
            FontWeight = model.FontWeight,
            TextDecoration = model.TextDecoration,
            MaxWidth = model.MaxWidth,
            BorderRadius = model.BorderRadius
        });

        TempData["Message"] = $"Utseendet för {ElementName(model.Element).ToLower()} sparades.";
        return RedirectToAction(nameof(Index));
    }

    private static SiteStyleFormViewModel Map(StyleElement element, SiteStyle? style) => new()
    {
        Element = element,
        ElementName = ElementName(element),
        FontFamily = style?.FontFamily,
        FontSize = style?.FontSize,
        Color = style?.Color,
        FontWeight = style?.FontWeight,
        TextDecoration = style?.TextDecoration,
        MaxWidth = style?.MaxWidth,
        BorderRadius = style?.BorderRadius
    };

    private static string ElementName(StyleElement element) => element switch
    {
        StyleElement.Heading => "Rubriker",
        StyleElement.Text => "Texter",
        StyleElement.Image => "Bilder",
        StyleElement.Link => "Länkar",
        _ => element.ToString()
    };
}
using Comasy.Core.Interfaces;
using Comasy.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Comasy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class PagesController(IPageService pageService) : Controller
{
    // Listar alla sidor, publicerade som opublicerade.
    public async Task<IActionResult> Index()
    {
        var pages = await pageService.GetAllPagesAsync();

        var model = pages
            .Select(p => new PageListItemViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Slug = p.Slug,
                IsPublished = p.IsPublished,
                CreatedAt = p.CreatedAt
            })
            .ToList();

        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreatePageViewModel());
    }

    // Servicen genererar slug och skapar sidan som opublicerad.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePageViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var page = await pageService.CreatePageAsync(model.Title);

        TempData["Message"] = $"Sidan \"{page.Title}\" skapades.";
        return RedirectToAction(nameof(Edit), new { id = page.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var page = await pageService.GetPageForEditAsync(id);

        if (page is null)
        {
            return NotFound();
        }

        return View(new EditPageViewModel
        {
            Id = page.Id,
            Title = page.Title,
            Slug = page.Slug,
            IsPublished = page.IsPublished
        });
    }

    // Sluggen ändras aldrig – den är sidans adress.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditPageViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var page = await pageService.GetPageForEditAsync(model.Id);

        if (page is null)
        {
            return NotFound();
        }

        page.Title = model.Title.Trim();
        page.IsPublished = model.IsPublished;

        await pageService.UpdatePageAsync(page);

        TempData["Message"] = "Ändringarna sparades.";
        return RedirectToAction(nameof(Index));
    }

    // Raderar sidan och dess innehållsblock via cascade i databasen.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var page = await pageService.GetPageForEditAsync(id);

        if (page is null)
        {
            return NotFound();
        }

        await pageService.DeletePageAsync(id);

        TempData["Message"] = $"Sidan \"{page.Title}\" raderades.";
        return RedirectToAction(nameof(Index));
    }
}
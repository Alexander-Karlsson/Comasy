using Comasy.Core.Entities;
using Comasy.Core.Interfaces;
using Comasy.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Comasy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class MenuController(IMenuService menuService, IPageService pageService) : Controller
{
    // Visar menyn som den är uppbyggd. Alltså toppnivå med undermenyer.
    public async Task<IActionResult> Index()
    {
        var all = (await menuService.GetAllAsync()).ToList();

        var topLevel = all
            .Where(m => m.ParentId is null)
            .OrderBy(m => m.SortOrder)
            .ToList();

        foreach (var parent in topLevel)
        {
            parent.Children = all
                .Where(m => m.ParentId == parent.Id)
                .OrderBy(m => m.SortOrder)
                .ToList();
        }

        return View(new MenuListViewModel { TopLevel = topLevel });
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new MenuItemFormViewModel();
        await PopulateListsAsync(model);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MenuItemFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateListsAsync(model);
            return View(model);
        }

        var item = await menuService.CreateAsync(model.Text, model.PageId, model.ParentId);

        if (!model.IsVisible)
        {
            item.IsVisible = false;
            await menuService.UpdateAsync(item);
        }

        TempData["Message"] = "Menyalternativet lades till.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await menuService.GetByIdAsync(id);

        if (item is null)
        {
            return NotFound();
        }

        var model = new MenuItemFormViewModel
        {
            Id = item.Id,
            Text = item.Text,
            PageId = item.PageId,
            ParentId = item.ParentId,
            IsVisible = item.IsVisible
        };

        await PopulateListsAsync(model);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MenuItemFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateListsAsync(model);
            return View(model);
        }

        var item = await menuService.GetByIdAsync(model.Id);

        if (item is null)
        {
            return NotFound();
        }

        item.Text = model.Text.Trim();
        item.PageId = model.PageId;
        item.ParentId = model.ParentId;
        item.IsVisible = model.IsVisible;

        await menuService.UpdateAsync(item);

        TempData["Message"] = "Ändringarna sparades.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MoveUp(int id)
    {
        await menuService.MoveUpAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MoveDown(int id)
    {
        await menuService.MoveDownAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await menuService.DeleteAsync(id);
            TempData["Message"] = "Menyalternativet togs bort.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    // Dropdownlistorna postas aldrig tillbaka och måste fyllas på nytt
    // varje gång vyn renderas.
    private async Task PopulateListsAsync(MenuItemFormViewModel model)
    {
        var pages = await pageService.GetAllPagesAsync();

        model.AvailablePages = pages
            .OrderBy(p => p.Title)
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.IsPublished ? p.Title : $"{p.Title} (utkast)"
            })
            .ToList();

        var menuItems = await menuService.GetAllAsync();

        model.AvailableParents = menuItems
            .Where(m => m.ParentId is null && m.Id != model.Id)
            .OrderBy(m => m.SortOrder)
            .Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Text
            })
            .ToList();
    }
}
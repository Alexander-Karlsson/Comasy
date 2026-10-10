using Comasy.Core.Entities;
using Comasy.Core.Enums;
using Comasy.Core.Interfaces;
using Comasy.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Comasy.Web.Areas.Admin.Controllers;


[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ContentController(IPageService pageService, IContentBlockService blockService) : Controller
{
    // Visar sidans innehållsblock i huvudzonen i sortordning.
    public async Task<IActionResult> Index(int id)
    {
        var page = await pageService.GetPageForEditAsync(id);
        
              if (page is null)
        {
            return NotFound();
        }

        var blocks = await blockService.GetForPageAsync(id, ContentZone.Main);

        return View(new ContentListViewModel
        {
            PageId = page.Id,
            PageTitle = page.Title,
            PageSlug = page.Slug,
            IsPublished = page.IsPublished,
            Blocks = blocks.ToList()
        });
    }

    // Visar formuläret för att lägga till ett nytt innehållsblock.
    [HttpGet]
    public async Task<IActionResult> Create(int pageId)
    {
        var page = await pageService.GetPageForEditAsync(pageId);

        if (page is null)
        {
            return NotFound();
        }

        return View(new ContentBlockFormViewModel
        {
            PageId = page.Id,
            PageTitle = page.Title
        });
    }

    // Validerar fälten efter blocktyp och skapar blocket på sidan.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContentBlockFormViewModel model)
    {
        ValidateByBlockType(model);

        if (!ModelState.IsValid)
        {
            await RepopulatePageTitleAsync(model);
            return View(model);
        }

        var block = new ContentBlock
        {
            PageId = model.PageId,
            Zone = ContentZone.Main,
            BlockType = model.BlockType,
            Text = model.Text?.Trim(),
            ImageUrl = model.ImageUrl?.Trim(),
            AltText = model.AltText?.Trim(),
            LinkUrl = model.LinkUrl?.Trim(),
            LinkText = model.LinkText?.Trim()
        };

        await blockService.CreateAsync(block);

        TempData["Message"] = "Innehållet lades till.";
        return RedirectToAction(nameof(Index), new { id = model.PageId });
    }

    // Visar formuläret ifyllt med det valda blockets nuvarande värden.
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var block = await blockService.GetByIdAsync(id);

        if (block is null)
        {
            return NotFound();
        }

        var page = await pageService.GetPageForEditAsync(block.PageId);

        return View(new ContentBlockFormViewModel
        {
            Id = block.Id,
            PageId = block.PageId,
            PageTitle = page?.Title ?? string.Empty,
            BlockType = block.BlockType,
            Text = block.Text,
            ImageUrl = block.ImageUrl,
            AltText = block.AltText,
            LinkUrl = block.LinkUrl,
            LinkText = block.LinkText
        });
    }

    // Sparar ändringar i ett befintligt block efter samma typvalidering som vid create.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ContentBlockFormViewModel model)
    {
        ValidateByBlockType(model);

        if (!ModelState.IsValid)
        {
            await RepopulatePageTitleAsync(model);
            return View(model);
        }

        var block = await blockService.GetByIdAsync(model.Id);

        if (block is null)
        {
            return NotFound();
        }

        block.BlockType = model.BlockType;
        block.Text = model.Text?.Trim();
        block.ImageUrl = model.ImageUrl?.Trim();
        block.AltText = model.AltText?.Trim();
        block.LinkUrl = model.LinkUrl?.Trim();
        block.LinkText = model.LinkText?.Trim();

        await blockService.UpdateAsync(block);

        TempData["Message"] = "Ändringarna sparades.";
        return RedirectToAction(nameof(Index), new { id = block.PageId });
    }

    // Flyttar blocket ett steg uppåt bland syskonen på sidan.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MoveUp(int id)
    {
        var block = await blockService.GetByIdAsync(id);

        if (block is null)
        {
            return NotFound();
        }

        await blockService.MoveUpAsync(id);
        return RedirectToAction(nameof(Index), new { id = block.PageId });
    }

    // Flyttar blocket ett steg nedåt bland syskonen på sidan.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MoveDown(int id)
    {
        var block = await blockService.GetByIdAsync(id);

        if (block is null)
        {
            return NotFound();
        }

        await blockService.MoveDownAsync(id);
        return RedirectToAction(nameof(Index), new { id = block.PageId });
    }

    // Tar bort blocket och går tillbaka till sidans innehållslista.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var block = await blockService.GetByIdAsync(id);

        if (block is null)
        {
            return NotFound();
        }

        var pageId = block.PageId;
        await blockService.DeleteAsync(id);

        TempData["Message"] = "Innehållet togs bort.";
        return RedirectToAction(nameof(Index), new { id = pageId });
    }

    // Olika blocktyper kräver olika fält. Det går inte att uttrycka
    // med data annotations, så kontrollen görs här.
    private void ValidateByBlockType(ContentBlockFormViewModel model)
    {
        switch (model.BlockType)
        {
            case BlockType.Heading:
            case BlockType.Text:
                if (string.IsNullOrWhiteSpace(model.Text))
                {
                    ModelState.AddModelError(nameof(model.Text), "Ange text");
                }
                break;

            case BlockType.Image:
                if (string.IsNullOrWhiteSpace(model.ImageUrl))
                {
                    ModelState.AddModelError(nameof(model.ImageUrl), "Ange en bildadress");
                }
                if (string.IsNullOrWhiteSpace(model.AltText))
                {
                    ModelState.AddModelError(nameof(model.AltText),
                        "Ange en alternativtext som beskriver bilden");
                }
                break;

            case BlockType.Link:
                if (string.IsNullOrWhiteSpace(model.LinkUrl))
                {
                    ModelState.AddModelError(nameof(model.LinkUrl), "Ange en länkadress");
                }
                if (string.IsNullOrWhiteSpace(model.LinkText))
                {
                    ModelState.AddModelError(nameof(model.LinkText), "Ange en länktext");
                }
                break;
        }
    }

    // Sidtiteln ligger inte i formuläret. Den fylls i igen om valideringen misslyckas.
    private async Task RepopulatePageTitleAsync(ContentBlockFormViewModel model)
    {
        var page = await pageService.GetPageForEditAsync(model.PageId);
        model.PageTitle = page?.Title ?? string.Empty;
    }
    
}
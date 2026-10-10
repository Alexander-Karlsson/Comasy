using Comasy.Core.Enums;
using Comasy.Core.Interfaces;
using Comasy.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Comasy.Web.Controllers;

public class PageController(IPageService pageService, IContentBlockService blockService, 
    IStatisticsService statisticsService) : Controller
{
    public async Task<IActionResult> Index(string slug)
    {
        var page = await pageService.GetPublishedPageAsync(slug);

        if (page is null)
        {
            return NotFound();
        }

        await statisticsService.RegisterViewAsync(page.Id);

        var model = new PageViewModel
        {
            Title = page.Title,
            MainBlocks = (await blockService.GetForPageAsync(page.Id, ContentZone.Main)).ToList(),
            ViewCount = await statisticsService.GetViewCountAsync(page.Id)
        };

        return View(model);
    }
}
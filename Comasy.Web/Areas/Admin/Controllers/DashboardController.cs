using Comasy.Core.Interfaces;
using Comasy.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Comasy.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardController(
    IPageService pageService,
    IMenuService menuService,
    IStatisticsService statisticsService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var pages = (await pageService.GetAllPagesAsync()).ToList();
        var menuItems = (await menuService.GetAllAsync()).ToList();

        return View(new DashboardViewModel
        {
            PageCount = pages.Count,
            PublishedCount = pages.Count(p => p.IsPublished),
            MenuItemCount = menuItems.Count,
            ViewsLastWeek = await statisticsService.GetViewsLastDaysAsync(7)
        });
    }
}
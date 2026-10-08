using Comasy.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Comasy.Web.ViewComponents;

public class MenuViewComponent(IMenuService menuService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var menu = await menuService.GetMenuAsync();
        return View(menu);
    }
}
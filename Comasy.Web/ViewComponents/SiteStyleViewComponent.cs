using Comasy.Core.Interfaces;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace Comasy.Web.ViewComponents;

public class SiteStyleViewComponent(ISiteStyleService styleService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var css = await styleService.GenerateCssAsync();
        return View("Default", (object)css);
    }
}
using Comasy.Core.Entities;

namespace Comasy.Web.Models;

public class ContentListViewModel
{
    public int PageId { get; set; }
    public string PageTitle { get; set; } = string.Empty;
    public string PageSlug { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public List<ContentBlock> Blocks { get; set; } = [];
}
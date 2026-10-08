using Comasy.Core.Entities;

namespace Comasy.Web.Models;

public class PageViewModel
{
    public string Title { get; set; } = string.Empty;
    public List<ContentBlock> MainBlocks { get; set; } = [];
}
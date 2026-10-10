using Comasy.Core.Entities;
using Comasy.Core.Enums;

namespace Comasy.Core.Interfaces;

public interface ISiteStyleService
{
    Task<IEnumerable<SiteStyle>> GetAllAsync();
    Task<SiteStyle?> GetByElementAsync(StyleElement element);
    Task SaveAsync(SiteStyle style);
    Task<string> GenerateCssAsync();
}
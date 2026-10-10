using Comasy.Core.Entities;
using Comasy.Core.Enums;

namespace Comasy.Core.Interfaces;

public interface ISiteStyleRepository
{
    Task<IEnumerable<SiteStyle>> GetAllAsync();
    Task<SiteStyle?> GetByElementAsync(StyleElement element);
    Task UpsertAsync(SiteStyle style);
}
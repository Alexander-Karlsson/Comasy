using Comasy.Api.Entities;

namespace Comasy.Api.Data;

public interface IPageViewRepository
{
    Task AddAsync(PageView pageView);
    Task<int> GetCountForPageAsync(int pageId);
    Task<Dictionary<int, int>> GetCountsPerPageAsync();
    Task<int> GetCountSinceAsync(DateTime fromUtc);
}
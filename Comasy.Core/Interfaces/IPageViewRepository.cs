using Comasy.Core.Entities;

namespace Comasy.Core.Interfaces;

public interface IPageViewRepository
{
    Task AddAsync(PageView pageView);
    Task<int> GetCountForPageAsync(int pageId);
    Task<Dictionary<int, int>> GetCountsPerPageAsync();
    Task<int> GetTotalCountSinceAsync(DateTime from);
}
using Comasy.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Comasy.Api.Data;

internal class PageViewRepository(StatisticsDbContext context) : IPageViewRepository
{
    public async Task AddAsync(PageView pageView)
    {
        context.PageViews.Add(pageView);
        await context.SaveChangesAsync();
    }

    public async Task<int> GetCountForPageAsync(int pageId)
        => await context.PageViews.CountAsync(x => x.PageId == pageId);

    public async Task<Dictionary<int, int>> GetCountsPerPageAsync()
    {
        return await context.PageViews
            .GroupBy(x => x.PageId)
            .Select(g => new { PageId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.PageId, x => x.Count);
    }

    public async Task<int> GetCountSinceAsync(DateTime fromUtc)
        => await context.PageViews.CountAsync(x => x.ViewedAt >= fromUtc);
    
}
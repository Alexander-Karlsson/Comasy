using Comasy.Core.Entities;
using Comasy.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Comasy.Data.Repositories;

public class PageViewRepository(ComasyDbContext context) : IPageViewRepository
{
    public async Task AddAsync(PageView pageView)
    {
        context.PageViews.Add(pageView);
        await context.SaveChangesAsync();
    }

    public async Task<int> GetCountForPageAsync(int pageId)
    {
        return await context.PageViews
            .AsNoTracking()
            .CountAsync(v => v.PageId == pageId);
    }

    public async Task<Dictionary<int, int>> GetCountsPerPageAsync()
    {
        return await context.PageViews
            .AsNoTracking()
            .GroupBy(v => v.PageId)
            .Select(g => new { PageId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.PageId, x => x.Count);
    }

    public async Task<int> GetTotalCountSinceAsync(DateTime from)
    {
        return await context.PageViews
            .AsNoTracking()
            .CountAsync(v => v.ViewedAt >= from);
    }
}
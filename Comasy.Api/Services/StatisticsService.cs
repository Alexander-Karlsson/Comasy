using Comasy.Api.Data;
using Comasy.Api.Entities;

namespace Comasy.Api.Services;

public class StatisticsService(IPageViewRepository repo) : IStatisticsService
{
    public async Task RegisterViewAsync(int pageId)
    {
        if (pageId <= 0) throw new ArgumentOutOfRangeException(
                nameof(pageId), "PageId måste vara ett positivt tal!");
        
        var pageView = new PageView
        {
            PageId = pageId,
            ViewedAt = DateTime.UtcNow 
        };

        await repo.AddAsync(pageView);
    }

    public async Task<int> GetViewCountAsync(int pageId)
    {
        if (pageId <= 0) throw new ArgumentOutOfRangeException(
                nameof(pageId), "PageId måste vara ett positivt tal!");
        

        return await repo.GetCountForPageAsync(pageId);
    }

    public async Task<Dictionary<int, int>> GetViewCountsAsync()
        => await repo.GetCountsPerPageAsync();

    public async Task<int> GetViewsLastDaysAsync(int days)
    {
        if (days <= 0) throw new ArgumentOutOfRangeException(
            nameof(days), "Antal dagar måste vara ett positivt tal!");
        
        var fromUtc =  DateTime.UtcNow.AddDays(-days);

        return await repo.GetCountSinceAsync(fromUtc);

    }

}
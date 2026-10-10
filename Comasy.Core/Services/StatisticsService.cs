using Comasy.Core.Entities;
using Comasy.Core.Interfaces;

namespace Comasy.Core.Services;

public class StatisticsService(IPageViewRepository pageViewRepository) : IStatisticsService
{
    public async Task RegisterViewAsync(int pageId)
    {
        try
        {
            await pageViewRepository.AddAsync(new PageView
            {
                PageId = pageId,
                ViewedAt = DateTime.UtcNow
            });
        }
        catch
        {
            // Sväljer undantag medvetet så att statistiken inte sabbar en sidvisning.
        }
    }

    public Task<int> GetViewCountAsync(int pageId)
        => pageViewRepository.GetCountForPageAsync(pageId);

    public Task<Dictionary<int, int>> GetViewCountsAsync()
        => pageViewRepository.GetCountsPerPageAsync();

    public Task<int> GetViewsLastDaysAsync(int days)
        => pageViewRepository.GetTotalCountSinceAsync(DateTime.UtcNow.AddDays(-days));
}
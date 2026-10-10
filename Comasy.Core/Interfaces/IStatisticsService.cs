namespace Comasy.Core.Interfaces;

public interface IStatisticsService
{
    Task RegisterViewAsync(int pageId);
    Task<int> GetViewCountAsync(int pageId);
    Task<Dictionary<int, int>> GetViewCountsAsync();
    Task<int> GetViewsLastDaysAsync(int days);
}
using Comasy.Core.Interfaces;
using System.Net.Http.Json;
namespace Comasy.Web.Services;

public class StatisticsApiClient(HttpClient httpClient, ILogger<StatisticsApiClient> logger) : IStatisticsService
{
    
    private record ViewCountDto(int PageId, int Count);

    private record RecentViewsDto(int Days, int Count);
    
    public async Task RegisterViewAsync(int pageId)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync(
                "api/statistics/views", new { pageId });

            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex,
                "Kunde inte registrera sidvisning för sida {PageId}.", pageId);
        }
    }

    public async Task<int> GetViewCountAsync(int pageId)
    {
        try
        {
            var result = await httpClient.GetFromJsonAsync<ViewCountDto>(
                $"api/statistics/views/{pageId}");

            return result?.Count ?? 0;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex,
                "Kunde inte hämta visningar för sida {PageId}.", pageId);

            return 0;
        }
    }

    public async Task<Dictionary<int, int>> GetViewCountsAsync()
    {
        try
        {
            var result = await httpClient.GetFromJsonAsync<List<ViewCountDto>>(
                "api/statistics/views");

            return result?.ToDictionary(item => item.PageId, item => item.Count)
                   ?? new Dictionary<int, int>();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Kunde inte hämta visningsstatistik.");

            return new Dictionary<int, int>();
        }
    }

    public async Task<int> GetViewsLastDaysAsync(int days)
    {
        try
        {
            var result = await httpClient.GetFromJsonAsync<RecentViewsDto>(
                $"api/statistics/views/recent?days={days}");

            return result?.Count ?? 0;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex,
                "Kunde inte hämta visningar för de senaste {Days} dagarna.", days);

            return 0;
        }
    }
}
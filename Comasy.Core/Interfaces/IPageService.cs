using Comasy.Core.Entities;

namespace Comasy.Core.Interfaces;

public interface IPageService
{
    Task<Page?> GetPublishedPageAsync(string slug);
    Task<IEnumerable<Page>> GetAllPagesAsync();
    Task<Page?> GetPageForEditAsync(int id);
    Task<Page> CreatePageAsync(string title);
    Task UpdatePageAsync(Page page);
    Task DeletePageAsync(int id);
}
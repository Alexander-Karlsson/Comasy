
using Comasy.Core.Entities;

namespace Comasy.Core.Interfaces
{
    public interface IPageRepository
    {
        Task<IEnumerable<Page>> GetAllAsync();
        Task<Page?> GetByIdAsync(int id);
        Task<Page?> GetBySlugAsync(string slug);
        Task<Page> AddAsync(Page page);
        Task UpdateAsync(Page page);
        Task DeleteAsync(int id);
    }
}

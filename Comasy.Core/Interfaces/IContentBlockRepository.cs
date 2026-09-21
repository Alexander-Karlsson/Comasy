using Comasy.Core.Entities;
using Comasy.Core.Enums;

namespace Comasy.Core.Interfaces
{
    public interface IContentBlockRepository
    {
        Task<IEnumerable<ContentBlock>> GetByPageAsync(int pageId, ContentZone zone);
        Task<ContentBlock?> GetByIdAsync(int id);
        Task<ContentBlock> AddAsync(ContentBlock contentBlock);
        Task UpdateAsync(ContentBlock contentBlock);
        Task DeleteAsync(int id);
    }
}

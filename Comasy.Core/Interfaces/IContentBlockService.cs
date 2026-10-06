using Comasy.Core.Entities;
using Comasy.Core.Enums;

namespace Comasy.Core.Interfaces;

public interface IContentBlockService
{
    Task<IEnumerable<ContentBlock>> GetForPageAsync(int pageId, ContentZone zone);
    Task<ContentBlock?> GetByIdAsync(int id);
    Task<ContentBlock> CreateAsync(ContentBlock block);
    Task UpdateAsync(ContentBlock block);
    Task DeleteAsync(int id);
    Task MoveUpAsync(int id);
    Task MoveDownAsync(int id);
}
using Comasy.Core.Entities;

namespace Comasy.Core.Interfaces;

public interface IMenuService
{
    Task<IEnumerable<MenuItem>> GetMenuAsync();
    Task<IEnumerable<MenuItem>> GetAllAsync();
    Task<MenuItem?> GetByIdAsync(int id);
    Task<MenuItem> CreateAsync(string text, int pageId, int? parentId);
    Task UpdateAsync(MenuItem menuItem);
    Task DeleteAsync(int id);
    Task MoveUpAsync(int id);
    Task MoveDownAsync(int id);
}
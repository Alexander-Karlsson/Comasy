using Comasy.Core.Entities;
using Comasy.Core.Interfaces;

namespace Comasy.Core.Services;

public class MenuService(IMenuItemRepository menuItemRepository) : IMenuService
{
    public Task<IEnumerable<MenuItem>> GetMenuAsync()
        => menuItemRepository.GetMenuAsync();

    public Task<IEnumerable<MenuItem>> GetAllAsync()
        => menuItemRepository.GetAllAsync();

    public Task<MenuItem?> GetByIdAsync(int id)
        => menuItemRepository.GetByIdAsync(id);

    public async Task<MenuItem> CreateAsync(string text, int pageId, int? parentId)
    {
        var siblings = (await menuItemRepository.GetAllAsync())
            .Where(m => m.ParentId == parentId)
            .ToList();

        var nextSortOrder = siblings.Count == 0
            ? 10
            : siblings.Max(m => m.SortOrder) + 10;

        var menuItem = new MenuItem
        {
            Text = text.Trim(),
            PageId = pageId,
            ParentId = parentId,
            SortOrder = nextSortOrder,
            IsVisible = true
        };

        return await menuItemRepository.AddAsync(menuItem);
    }

    public Task UpdateAsync(MenuItem menuItem)
        => menuItemRepository.UpdateAsync(menuItem);

    public async Task DeleteAsync(int id)
    {
        var hasChildren = (await menuItemRepository.GetAllAsync())
            .Any(m => m.ParentId == id);

        if (hasChildren)
        {
            throw new InvalidOperationException(
                "Menyposten har undermenyer. Flytta eller radera dem först.");
        }

        await menuItemRepository.DeleteAsync(id);
    }

    public Task MoveUpAsync(int id) => MoveAsync(id, -1);

    public Task MoveDownAsync(int id) => MoveAsync(id, +1);

    private async Task MoveAsync(int id, int direction)
    {
        var all = (await menuItemRepository.GetAllAsync()).ToList();

        var current = all.FirstOrDefault(m => m.Id == id);
        if (current is null) return;

        var siblings = all
            .Where(m => m.ParentId == current.ParentId)
            .OrderBy(m => m.SortOrder)
            .ToList();

        var index = siblings.FindIndex(m => m.Id == id);
        var targetIndex = index + direction;

        if (targetIndex < 0 || targetIndex >= siblings.Count) return;

        var a = await menuItemRepository.GetByIdAsync(id);
        var b = await menuItemRepository.GetByIdAsync(siblings[targetIndex].Id);
        if (a is null || b is null) return;

        (a.SortOrder, b.SortOrder) = (b.SortOrder, a.SortOrder);

        await menuItemRepository.UpdateAsync(a);
        await menuItemRepository.UpdateAsync(b);
    }
}
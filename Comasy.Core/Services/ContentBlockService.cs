using Comasy.Core.Entities;
using Comasy.Core.Enums;
using Comasy.Core.Interfaces;

namespace Comasy.Core.Services;

public class ContentBlockService(IContentBlockRepository blockRepository)
    : IContentBlockService
{
    public Task<IEnumerable<ContentBlock>> GetForPageAsync(int pageId, ContentZone zone)
        => blockRepository.GetByPageAsync(pageId, zone);

    public Task<ContentBlock?> GetByIdAsync(int id)
        => blockRepository.GetByIdAsync(id);

    public async Task<ContentBlock> CreateAsync(ContentBlock block)
    {
        var existing = (await blockRepository
            .GetByPageAsync(block.PageId, block.Zone)).ToList();

        block.SortOrder = existing.Count == 0
            ? 10
            : existing.Max(b => b.SortOrder) + 10;

        return await blockRepository.AddAsync(block);
    }

    public Task UpdateAsync(ContentBlock block)
        => blockRepository.UpdateAsync(block);

    public Task DeleteAsync(int id)
        => blockRepository.DeleteAsync(id);

    public Task MoveUpAsync(int id) => MoveAsync(id, -1);

    public Task MoveDownAsync(int id) => MoveAsync(id, +1);

    private async Task MoveAsync(int id, int direction)
    {
        var current = await blockRepository.GetByIdAsync(id);
        if (current is null) return;

        var siblings = (await blockRepository
            .GetByPageAsync(current.PageId, current.Zone)).ToList();

        var index = siblings.FindIndex(b => b.Id == id);
        var targetIndex = index + direction;

        if (targetIndex < 0 || targetIndex >= siblings.Count) return;

        var neighbour = await blockRepository.GetByIdAsync(siblings[targetIndex].Id);
        if (neighbour is null) return;

        (current.SortOrder, neighbour.SortOrder) =
            (neighbour.SortOrder, current.SortOrder);

        await blockRepository.UpdateAsync(current);
        await blockRepository.UpdateAsync(neighbour);
    }
}
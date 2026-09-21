using Comasy.Core.Entities;
using Comasy.Core.Enums;
using Comasy.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Comasy.Data.Repositories
{
    internal class ContentBlockRepository(ComasyDbContext context) : IContentBlockRepository
    {
        public async Task<ContentBlock> AddAsync(ContentBlock contentBlock)
        {
            context.ContentBlocks.Add(contentBlock);
            await context.SaveChangesAsync();
            return contentBlock;
        }

        public async Task DeleteAsync(int id)
        {
            var block = await context.ContentBlocks.FindAsync(id);
            if (block == null) return;

            context.ContentBlocks.Remove(block);
            await context.SaveChangesAsync();
        }

        public async Task<ContentBlock?> GetByIdAsync(int id)
        {
            return await context.ContentBlocks
                .FirstOrDefaultAsync(b => b.Id == id);
                
        }

        public async Task<IEnumerable<ContentBlock>> GetByPageAsync(int pageId, ContentZone zone)
        {
            return await context.ContentBlocks
                .AsNoTracking()
                .Where(b => b.PageId == pageId && b.Zone == zone)
                .OrderBy(b => b.SortOrder)
                .ToListAsync();
        }

        public async Task UpdateAsync(ContentBlock contentBlock)
        {
            context.ContentBlocks.Update(contentBlock);
            await context.SaveChangesAsync();
        }
    }
}

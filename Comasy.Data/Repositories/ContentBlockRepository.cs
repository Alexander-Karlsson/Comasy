using Comasy.Core.Entities;
using Comasy.Core.Enums;
using Comasy.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Comasy.Data.Repositories
{
    internal class ContentBlockRepository(ComasyDbContext context) : IContentBlockRepository
    {
        /// <summary>
        /// Adds a new content block and saves it to the database.
        /// </summary>
        /// <param name="contentBlock">The content block to add.</param>
        /// <returns>The added content block, with its id set by the database.</returns>
        public async Task<ContentBlock> AddAsync(ContentBlock contentBlock)
        {
            context.ContentBlocks.Add(contentBlock);
            await context.SaveChangesAsync();
            return contentBlock;
        }

        /// <summary>
        /// Deletes the content block with the given id. Does nothing if no such content block exists.
        /// </summary>
        /// <param name="id">The id of the content block to delete.</param>
        public async Task DeleteAsync(int id)
        {
            var block = await context.ContentBlocks.FindAsync(id);
            if (block == null) return;

            context.ContentBlocks.Remove(block);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the content block with the given id.
        /// </summary>
        /// <param name="id">The id of the content block to get.</param>
        /// <returns>The content block if it exists; otherwise, null.</returns>
        public async Task<ContentBlock?> GetByIdAsync(int id)
        {
            return await context.ContentBlocks
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        /// <summary>
        /// Gets the content blocks in the given zone of a page, ordered by sort order.
        /// </summary>
        /// <remarks>
        /// The content blocks are not tracked by the context, so changes to them are not saved.
        /// </remarks>
        /// <param name="pageId">The id of the page the content blocks belong to.</param>
        /// <param name="zone">The zone of the page to get content blocks from.</param>
        /// <returns>The matching content blocks, or an empty collection if there are none.</returns>
        public async Task<IEnumerable<ContentBlock>> GetByPageAsync(int pageId, ContentZone zone)
        {
            return await context.ContentBlocks
                .AsNoTracking()
                .Where(b => b.PageId == pageId && b.Zone == zone)
                .OrderBy(b => b.SortOrder)
                .ToListAsync();
        }

        /// <summary>
        /// Updates an existing content block and saves the changes to the database.
        /// </summary>
        /// <param name="contentBlock">The content block with the updated values.</param>
        public async Task UpdateAsync(ContentBlock contentBlock)
        {
            context.ContentBlocks.Update(contentBlock);
            await context.SaveChangesAsync();
        }
    }
}
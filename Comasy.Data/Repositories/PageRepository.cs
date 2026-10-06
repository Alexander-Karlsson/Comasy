using Comasy.Core.Entities;
using Comasy.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Comasy.Data.Repositories
{
    internal class PageRepository(ComasyDbContext context) : IPageRepository
    {
        /// <summary>
        /// Adds a new page and saves it to the database.
        /// </summary>
        /// <param name="page">The page to add.</param>
        /// <returns>The added page, with its id set by the database.</returns>
        public async Task<Page> AddAsync(Page page)
        {
            context.Pages.Add(page);
            await context.SaveChangesAsync();
            return page;
        }

        /// <summary>
        /// Deletes the page with the given id. Does nothing if no such page exists.
        /// </summary>
        /// <param name="id">The id of the page to delete.</param>
        public async Task DeleteAsync(int id)
        {
            var page = await context.Pages.FindAsync(id);
            if (page == null) return;

            context.Pages.Remove(page);
            await context.SaveChangesAsync();

        }

        /// <summary>
        /// Gets all pages ordered by title, without their content blocks.
        /// </summary>
        /// <remarks>
        /// The pages are not tracked by the context, so changes to them are not saved.
        /// </remarks>
        /// <returns>All pages, both published and unpublished.</returns>
        public async Task<IEnumerable<Page>> GetAllAsync()
        {
            return await context.Pages
                .AsNoTracking()
                .OrderBy(p => p.Title)
                .ToListAsync();
        }

        /// <summary>
        /// Gets the page with the given id, including its content blocks.
        /// </summary>
        /// <param name="id">The id of the page to get.</param>
        /// <returns>The page if it exists, published or not; otherwise, null.</returns>
        public async Task<Page?> GetByIdAsync(int id)
        {
            return await context.Pages
                .Include(p => p.ContentBlocks)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Gets the published page with the given slug, including its content blocks.
        /// </summary>
        /// <param name="slug">The slug of the page to get.</param>
        /// <returns>The page if it exists and is published; otherwise, null.</returns>
        public async Task<Page?> GetBySlugAsync(string slug)
        {
            return await context.Pages
                .Include(p => p.ContentBlocks)
                .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);
        }

        /// <summary>
        /// Checks if a page with the given slug already exists.
        /// </summary>
        /// <param name="slug">The slug to check.</param>
        /// <returns>True if a page with the slug exists; otherwise, false.</returns> 
        public async Task<bool> SlugExistsAsync(string slug)
        {
            return await context.Pages.AnyAsync(p => p.Slug == slug);
        }

        /// <summary>
        /// Updates an existing page and saves the changes to the database.
        /// </summary>
        /// <param name="page">The page with the updated values.</param>
        public async Task UpdateAsync(Page page)
        {
            context.Pages.Update(page);
            await context.SaveChangesAsync();
        }
    }
}

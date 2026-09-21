using Comasy.Core.Entities;
using Comasy.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Comasy.Data.Repositories
{
    internal class PageRepository(ComasyDbContext context) : IPageRepository
    {
        public async Task<Page> AddAsync(Page page)
        {
            context.Pages.Add(page);
            await context.SaveChangesAsync();
            return page;
        }

        public async Task DeleteAsync(int id)
        {
            var page = await context.Pages.FindAsync(id);
            if (page == null) return;

            context.Pages.Remove(page);
            await context.SaveChangesAsync();

        }

        public async Task<IEnumerable<Page>> GetAllAsync()
        {
            return await context.Pages
                .AsNoTracking()
                .OrderBy(p => p.Title)
                .ToListAsync();
        }

        public async Task<Page?> GetByIdAsync(int id)
        {
            return await context.Pages
                .Include(p => p.ContentBlocks)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Page?> GetBySlugAsync(string slug)
        {
            return await context.Pages
                .Include(p => p.ContentBlocks)
                .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);
        }

        public async Task UpdateAsync(Page page)
        {
            context.Pages.Update(page);
            await context.SaveChangesAsync();
        }
    }
}

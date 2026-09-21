
using Comasy.Core.Entities;
using Comasy.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Comasy.Data.Repositories
{
    internal class MenuItemRepository(ComasyDbContext context) : IMenuItemRepository
    {
        public async Task<MenuItem> AddAsync(MenuItem menuItem)
        {
            context.MenuItems.Add(menuItem);
            await context.SaveChangesAsync();
            return menuItem;
        }

        public async Task DeleteAsync(int id)
        {
            var menuItem = await context.MenuItems.FindAsync(id);
            if (menuItem == null) return;

            context.MenuItems.Remove(menuItem);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            return await context.MenuItems
                .AsNoTracking()
                .Include(m => m.Page)
                .OrderBy(m => m.ParentId)
                .ThenBy(m => m.SortOrder)
                .ToListAsync(); 
        }

        public async Task<MenuItem?> GetByIdAsync(int id)
        {
            return await context.MenuItems.FirstOrDefaultAsync(m => m.Id == id);  
        }

        public async Task<IEnumerable<MenuItem>> GetMenuAsync()
        {
            return await context.MenuItems
                .AsNoTracking()
                .Where(m => m.IsVisible && m.ParentId == null)
                .Include(m => m.Children
                    .Where(c => c.IsVisible)
                    .OrderBy(c => c.SortOrder))
                .OrderBy(m => m.SortOrder)
                .ToListAsync();
        }

        public async Task UpdateAsync(MenuItem menuItem)
        {
            context.MenuItems.Update(menuItem);
            await context.SaveChangesAsync();
        }
    }
}

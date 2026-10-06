
using Comasy.Core.Entities;
using Comasy.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Comasy.Data.Repositories
{
    internal class MenuItemRepository(ComasyDbContext context) : IMenuItemRepository
    {
        /// <summary>
        /// Adds a new menu item and saves it to the database.
        /// </summary>
        /// <param name="menuItem">The menu item to add.</param>
        /// <returns>The added menu item, with its id set by the database.</returns>
        public async Task<MenuItem> AddAsync(MenuItem menuItem)
        {
            context.MenuItems.Add(menuItem);
            await context.SaveChangesAsync();
            return menuItem;
        }

        /// <summary>
        /// Deletes the menu item with the given id. Does nothing if no such menu item exists.
        /// </summary>
        /// <param name="id">The id of the menu item to delete.</param>
        public async Task DeleteAsync(int id)
        {
            var menuItem = await context.MenuItems.FindAsync(id);
            if (menuItem == null) return;

            context.MenuItems.Remove(menuItem);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets all menu items with their linked pages, ordered by parent and then by sort order.
        /// </summary>
        /// <remarks>
        /// The menu items are not tracked by the context, so changes to them are not saved.
        /// </remarks>
        /// <returns>All menu items as a flat list, both visible and hidden.</returns>
        public async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            return await context.MenuItems
                .AsNoTracking()
                .Include(m => m.Page)
                .OrderBy(m => m.ParentId)
                .ThenBy(m => m.SortOrder)
                .ToListAsync(); 
        }

        /// <summary>
        /// Gets the menu item with the given id, without its linked page or children.
        /// </summary>
        /// <param name="id">The id of the menu item to get.</param>
        /// <returns>The menu item if it exists; otherwise, null.</returns>
        public async Task<MenuItem?> GetByIdAsync(int id)
        {
            return await context.MenuItems.FirstOrDefaultAsync(m => m.Id == id);  
        }

        /// <summary>
        /// Gets the visible top-level menu items with their visible children, ordered by sort order.
        /// Menu items linked to an unpublished page are excluded.
        /// </summary>
        /// <remarks>
        /// Linked pages are included. Only one level of children is loaded.
        /// The menu items are not tracked by the context.
        /// </remarks>
        /// <returns>The visible top-level menu items, each with its visible children.</returns>
        public async Task<IEnumerable<MenuItem>> GetMenuAsync()
        {
            return await context.MenuItems
                .AsNoTracking()
                .Where(m => m.IsVisible && m.ParentId == null)
                .Where(m => m.Page == null || m.Page.IsPublished)
                .Include(m => m.Page)
                .Include(m => m.Children
                    .Where(c => c.IsVisible && (c.Page == null || c.Page.IsPublished))
                    .OrderBy(c => c.SortOrder))
                .ThenInclude(c => c.Page)
                .OrderBy(m => m.SortOrder)
                .ToListAsync();
        }
        
        /// <summary>
        /// Updates an existing menu item and saves the changes to the database.
        /// </summary>
        /// <param name="menuItem">The menu item with the updated values.</param>
        public async Task UpdateAsync(MenuItem menuItem)
        {
            context.MenuItems.Update(menuItem);
            await context.SaveChangesAsync();
        }
    }
}

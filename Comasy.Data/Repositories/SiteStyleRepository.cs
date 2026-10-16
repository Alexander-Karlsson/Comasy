using Comasy.Core.Entities;
using Comasy.Core.Enums;
using Comasy.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Comasy.Data.Repositories;

internal class SiteStyleRepository(ComasyDbContext context) : ISiteStyleRepository
{
    public async Task<IEnumerable<SiteStyle>> GetAllAsync()
        => await context.SiteStyles
            .AsNoTracking()
            .OrderBy(s => s.Element)
            .ToListAsync();
    

    public async Task<SiteStyle?> GetByElementAsync(StyleElement element)
        => await context.SiteStyles
            .FirstOrDefaultAsync(s => s.Element == element);
    

    public async Task UpsertAsync(SiteStyle style)
    {
        var existing = await context.SiteStyles
            .FirstOrDefaultAsync(s => s.Element == style.Element);

        if (existing is null)
        {
            context.SiteStyles.Add(style);
        }
        else
        {
            existing.FontFamily = style.FontFamily;
            existing.FontSize = style.FontSize;
            existing.Color = style.Color;
            existing.FontWeight = style.FontWeight;
            existing.TextDecoration = style.TextDecoration;
            existing.MaxWidth = style.MaxWidth;
            existing.BorderRadius = style.BorderRadius;
        }

        await context.SaveChangesAsync();
    }
}
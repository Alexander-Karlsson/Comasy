using System.Text.RegularExpressions;
using Comasy.Core.Entities;
using Comasy.Core.Interfaces;

namespace Comasy.Core.Services;

public class PageService(IPageRepository pageRepository) : IPageService
{
    public Task<Page?> GetPublishedPageAsync(string slug)
        => pageRepository.GetBySlugAsync(slug);
    

    public Task<IEnumerable<Page>> GetAllPagesAsync() 
        => pageRepository.GetAllAsync();
   

    public Task<Page?> GetPageForEditAsync(int id)
        => pageRepository.GetByIdAsync(id);

    public async Task<Page> CreatePageAsync(string title)
    {
        var page = new Page
        {
            Title = title.Trim(),
            Slug = await GenerateUniqueSlugAsync(title),
            IsPublished = false,
            CreatedAt = DateTime.UtcNow
        };

        return await pageRepository.AddAsync(page);
    }

    public Task UpdatePageAsync(Page page)
        => pageRepository.UpdateAsync(page);

    public Task DeletePageAsync(int id)
        => pageRepository.DeleteAsync(id);
    
    private async Task<string> GenerateUniqueSlugAsync(string title)
    {
        var baseSlug = GenerateSlug(title);
        var slug = baseSlug;
        var counter = 2;

        while (await pageRepository.SlugExistsAsync(slug))
        {
            slug = $"{baseSlug}-{counter}";
            counter++;
        }

        return slug;
    }
    
    private static string GenerateSlug(string title)
    {
        var text = title.ToLowerInvariant()
            .Replace("å", "a")
            .Replace("ä", "a")
            .Replace("ö", "o");

        text = Regex.Replace(text, @"[^a-z0-9\s-]", "");
        text = Regex.Replace(text, @"\s+", "-");
        text = Regex.Replace(text, @"-+", "-");

        return text.Trim('-');
    }
}
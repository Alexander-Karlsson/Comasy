using Comasy.Core.Entities;
using Comasy.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Comasy.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(
        ComasyDbContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        string adminPassword)
    {
        await context.Database.MigrateAsync();

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        const string adminEmail = "admin@comasy.se";
        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin is null)
        {
            admin = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }

        if (await context.Pages.AnyAsync()) return;

        var start = new Page
        {
            Title = "Välkommen",
            Slug = "valkommen",
            IsPublished = true,
            ContentBlocks =
            [
                new ContentBlock
                {
                    BlockType = BlockType.Heading,
                    Zone = ContentZone.Main,
                    SortOrder = 10,
                    Text = "Välkommen till Comasy!"
                },
                new ContentBlock
                {
                    BlockType = BlockType.Text,
                    Zone = ContentZone.Main,
                    SortOrder = 20,
                    Text = "Den här sidan finns inte som en fil i projektet. " +
                           "Den är rader i en databas, renderade av en enda vy."
                }
            ]
        };

        var about = new Page
        {
            Title = "Om systemet",
            Slug = "om-systemet",
            IsPublished = true,
            ContentBlocks =
            [
                new ContentBlock
                {
                    BlockType = BlockType.Heading,
                    Zone = ContentZone.Main,
                    SortOrder = 10,
                    Text = "Om systemet"
                },
                new ContentBlock
                {
                    BlockType = BlockType.Text,
                    Zone = ContentZone.Main,
                    SortOrder = 20,
                    Text = "Comasy är ett av världens bästa publiceringssystem."
                },
                new ContentBlock
                {
                    BlockType = BlockType.Link,
                    Zone = ContentZone.Main,
                    SortOrder = 30,
                    LinkUrl = "/valkommen",
                    LinkText = "Tillbaka till startsidan"
                }
            ]
        };

        context.Pages.AddRange(start, about);
        await context.SaveChangesAsync();

        context.MenuItems.AddRange(
            new MenuItem { Text = "Start", PageId = start.Id, SortOrder = 10, IsVisible = true },
            new MenuItem { Text = "Om", PageId = about.Id, SortOrder = 20, IsVisible = true });

        await context.SaveChangesAsync();
    }
}
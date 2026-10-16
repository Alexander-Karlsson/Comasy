using Comasy.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Comasy.Api.Data;

public class StatisticsDbContext(DbContextOptions<StatisticsDbContext> options) : DbContext(options)
{
    public DbSet<PageView> PageViews => Set<PageView>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Sätter tabellnamnet då det är ett medvetet val att det är ett redan existerande
        // namn på en tabell. 
        builder.Entity<PageView>().ToTable("PageViews");
        builder.Entity<PageView>().HasIndex(x => x.PageId);
    }
}
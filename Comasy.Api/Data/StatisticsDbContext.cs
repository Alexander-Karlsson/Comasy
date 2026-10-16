using Comasy.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Comasy.Api.Data;

public class StatisticsDbContext : DbContext
{
    public StatisticsDbContext(DbContextOptions<StatisticsDbContext> options) : base(options) { }

    public DbSet<PageView> PageViews => Set<PageView>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<PageView>().ToTable("PageViews", "stats");
        builder.Entity<PageView>().HasIndex(v => v.PageId);
    }
}
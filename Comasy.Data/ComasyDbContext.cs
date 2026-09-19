using Comasy.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Comasy.Data
{
    public class ComasyDbContext : IdentityDbContext<IdentityUser>
    {
        public ComasyDbContext(DbContextOptions<ComasyDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Page> Pages => Set<Page>();
        public DbSet<ContentBlock> ContentBlocks => Set<ContentBlock>();
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();
        public DbSet<PageView> PageViews => Set<PageView>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Page>()
                .HasIndex(p => p.Slug)
                .IsUnique();

            builder.Entity<ContentBlock>()
                .HasOne(b => b.Page)
                .WithMany(b => b.ContentBlocks)
                .HasForeignKey(b => b.PageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MenuItem>()
                .HasOne(m => m.Parent)
                .WithMany(m => m.Children)
                .HasForeignKey(m => m.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
        
}

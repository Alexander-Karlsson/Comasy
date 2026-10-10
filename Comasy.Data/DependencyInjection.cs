using Comasy.Core.Interfaces;
using Comasy.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Comasy.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataLayer(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ComasyDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    // Azure SQL kan tappa eller strypa anslutningar en kort stund (failover,
                    // lastbalansering, tillfälliga nätverksstörningar). Det är transienta fel:
                    // en ny försökning lyckas oftast. EnableRetryOnFailure gör att EF Core
                    // kör om kommandot i stället för att begäran misslyckas direkt.
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null)));

            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            services.AddScoped<IContentBlockRepository, ContentBlockRepository>();
            services.AddScoped<IPageViewRepository, PageViewRepository>();


            return services;
        }
    }
}

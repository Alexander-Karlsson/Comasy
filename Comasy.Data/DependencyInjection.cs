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
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IPageRepository, PageRepository>();


            return services;
        }
    }
}

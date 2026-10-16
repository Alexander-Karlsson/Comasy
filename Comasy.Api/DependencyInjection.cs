using Comasy.Api.Data;
using Comasy.Api.Entities;
using Comasy.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Comasy.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddStatisticsServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StatisticsDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("StatisticsConnection"),
                sqlOptions =>
                {
                    sqlOptions.MigrationsHistoryTable("__EFMigrationsHistoryStatistics");

                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                }));

        services.AddScoped<IPageViewRepository, PageViewRepository>();
        services.AddScoped<IStatisticsService, StatisticsService>();

        return services;
    }
}
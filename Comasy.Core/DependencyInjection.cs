using Comasy.Core.Interfaces;
using Comasy.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Comasy.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddServiceLayer(this IServiceCollection services)
    {
        services.AddScoped<IPageService, PageService>();
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<IContentBlockService, ContentBlockService>();
        services.AddScoped<IStatisticsService,  StatisticsService>();
        return services;
    }
}
using DesafioViagens.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DesafioViagens.Domain;

public static class DependencyInjectionConfig
{
    public static IServiceCollection ResolveDependenciesDomain(this IServiceCollection services)
    {
        services.AddScoped<IRouteCalculator, DijkstraRouteCalculator>();
        services.AddScoped<IFindBestRouteService, FindBestRouteService>();
        services.AddScoped<IRouteService, RouteService>();


        return services;
    }
}
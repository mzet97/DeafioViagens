using DesafioViagens.Domain.Entities;
using DesafioViagens.Domain.Repositories;
using DesafioViagens.Domain.ViewModels;

namespace DesafioViagens.Domain.Services;

public class FindBestRouteService : IFindBestRouteService
{
    private readonly IRouteRepository _routeRepository;
    private readonly IRouteCalculator _routeCalculator;

    public FindBestRouteService(
        IRouteRepository routeRepository, 
        IRouteCalculator routeCalculator)
    {
        _routeRepository = routeRepository;
        _routeCalculator = routeCalculator;
    }

    public FindBestRouteResult FindBestRoute(string originCode, string destinationCode)
    {
        var routes = _routeRepository.GetAllRoutes();

        var origin = new Airport(originCode);
        var destination = new Airport(destinationCode);

        var (cost, path) = _routeCalculator.CalculateBestRoute(routes, origin, destination);

        return new FindBestRouteResult
        {
            Cost = cost,
            Path = path
        };
    }
}
using DesafioViagens.Domain.Entities;

namespace DesafioViagens.Domain.Services;

public interface IRouteCalculator
{
    (int cost, List<Airport> path) CalculateBestRoute(
        IEnumerable<Route> routes,
        Airport origin,
        Airport destination
    );
}

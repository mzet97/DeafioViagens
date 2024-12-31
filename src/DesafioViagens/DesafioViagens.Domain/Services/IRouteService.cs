using DesafioViagens.Domain.Entities;

namespace DesafioViagens.Domain.Services;

public interface IRouteService
{
    void CreateRoute(Route newRoute);
    void CreateRoute(IEnumerable<Route> routes);
    IEnumerable<Route> GetAllRoutes();
    Route GetRoute(string origin, string destination);
    bool UpdateRoute(string origin, string destination, Route updated);
    bool DeleteRoute(string origin, string destination);
}

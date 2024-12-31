using DesafioViagens.Domain.Entities;

namespace DesafioViagens.Domain.Repositories;

public interface IRouteRepository
{
    IEnumerable<Route> GetAllRoutes();
    void AddRoute(Route route);
    void UpdateRoute(string originCode, string destinationCode, Route updatedRoute);
    bool DeleteRoute(string originCode, string destinationCode);
    Route? GetRoute(string origin, string destination);
}
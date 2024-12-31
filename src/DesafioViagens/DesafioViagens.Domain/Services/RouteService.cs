using DesafioViagens.Domain.Entities;
using DesafioViagens.Domain.Exceptions;
using DesafioViagens.Domain.Repositories;

namespace DesafioViagens.Domain.Services;

public class RouteService(IRouteRepository _routeRepository) : IRouteService
{
    public void CreateRoute(Route newRoute)
    {
        if(newRoute == null)
            throw new ArgumentNullException(nameof(newRoute));

        var existes = _routeRepository.GetRoute(newRoute.Origin.Code, newRoute.Destination.Code);

        if (existes != null)
        {
            throw new ValidationException(@$"It already exists: {newRoute}");
        }

        if(newRoute.Cost <= 0)
        {
            throw new ValidationException("The cost must be greater than zero.");
        }

        if(newRoute.Origin == null)
        {
            throw new ValidationException("The origin airport is required.");
        }

        if (string.IsNullOrEmpty(newRoute.Origin.Code))
        {
            throw new ValidationException("The origin airport code is required.");
        }

        if (newRoute.Destination == null)
        {
            throw new ValidationException("The destination airport is required.");
        }

        if (string.IsNullOrEmpty(newRoute.Destination.Code))
        {
            throw new ValidationException("The destination airport code is required.");
        }

        _routeRepository.AddRoute(newRoute);
    }

    public void CreateRoute(IEnumerable<Route> routes)
    {
        foreach (var route in routes)
        {
            CreateRoute(route);
        }
    }

    public bool DeleteRoute(string origin, string destination)
    {
        var route = _routeRepository.GetRoute(origin, destination);

        if (route == null)
        {
            return false;
        }

        return _routeRepository.DeleteRoute(origin, destination);
    }

    public IEnumerable<Route> GetAllRoutes()
    {
        return _routeRepository.GetAllRoutes();
    }

    public Route GetRoute(string origin, string destination)
    {
        var route = _routeRepository.GetRoute(origin, destination);

        if(route == null)
        {
            throw new NotFoundException($"Route not found: {origin} -> {destination}");
        }

        return route;
    }

    public bool UpdateRoute(string origin, string destination, Route updated)
    {
        var route = _routeRepository.GetRoute(origin, destination);

        if (route == null)
        {
            throw new ValidationException(@$"Not exists route: {route}");
        }

        if(route.Origin.Code == updated.Origin.Code && 
            route.Destination.Code == updated.Destination.Code && 
            route.Cost == updated.Cost)
        {
            return false;
        }

        if (updated.Cost <= 0)
        {
            throw new ValidationException("The cost must be greater than zero.");
        }

        if (updated.Origin == null)
        {
            throw new ValidationException("The origin airport is required.");
        }

        if (string.IsNullOrEmpty(updated.Origin.Code))
        {
            throw new ValidationException("The origin airport code is required.");
        }

        if (updated.Destination == null)
        {
            throw new ValidationException("The destination airport is required.");
        }

        if (string.IsNullOrEmpty(updated.Destination.Code))
        {
            throw new ValidationException("The destination airport code is required.");
        }

        _routeRepository.UpdateRoute(origin, destination, updated);

        return true;
    }
}

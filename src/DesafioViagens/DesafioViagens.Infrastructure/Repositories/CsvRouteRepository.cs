using DesafioViagens.Domain.Entities;
using DesafioViagens.Domain.Repositories;

namespace DesafioViagens.Infrastructure.Repositories;

public class CsvRouteRepository : IRouteRepository
{
    private readonly string _csvPath;

    public CsvRouteRepository(string csvPath)
    {
        _csvPath = csvPath;
    }

    public IEnumerable<Route> GetAllRoutes()
    {
        var routes = new List<Route>();

        if (!File.Exists(_csvPath))
            return routes;

        foreach (var line in File.ReadAllLines(_csvPath))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split(',');
            if (parts.Length != 3)
                continue;

            var origin = new Airport(parts[0].Trim());
            var destination = new Airport(parts[1].Trim());

            if (!int.TryParse(parts[2].Trim(), out int cost))
                continue;

            routes.Add(new Route(origin, destination, cost));
        }

        return routes;
    }

    public void AddRoute(Route route)
    {
        var line = $"\n{route.Origin.Code},{route.Destination.Code},{route.Cost}";
        File.AppendAllText(_csvPath, line + Environment.NewLine);
    }

    public void UpdateRoute(string originCode, string destinationCode, Route updatedRoute)
    {
        var allRoutes = GetAllRoutes().ToList();

        var existing = allRoutes.FirstOrDefault(r =>
            r.Origin.Code.Equals(originCode, StringComparison.OrdinalIgnoreCase) &&
            r.Destination.Code.Equals(destinationCode, StringComparison.OrdinalIgnoreCase));

        if (existing == null)
        {
            throw new KeyNotFoundException($"Não foi encontrada rota {originCode} -> {destinationCode} para atualizar.");
        }

        allRoutes.Remove(existing);

        allRoutes.Add(updatedRoute);

        RewriteCsv(allRoutes);
    }

    public bool DeleteRoute(string originCode, string destinationCode)
    {
        var allRoutes = GetAllRoutes().ToList();

        var toDelete = allRoutes.FirstOrDefault(r =>
            r.Origin.Code.Equals(originCode, StringComparison.OrdinalIgnoreCase) &&
            r.Destination.Code.Equals(destinationCode, StringComparison.OrdinalIgnoreCase));

        if (toDelete == null)
        {
            return false;
        }

        allRoutes.Remove(toDelete);

        RewriteCsv(allRoutes);

        return true;
    }

    private void RewriteCsv(IEnumerable<Route> routes)
    {
        using var writer = new StreamWriter(_csvPath, false);
        foreach (var route in routes)
        {
            writer.WriteLine($"{route.Origin.Code},{route.Destination.Code},{route.Cost}");
        }
    }

    public Route? GetRoute(string origin, string destination)
    {
       return GetAllRoutes().FirstOrDefault(r =>
            r.Origin.Code.Equals(origin, StringComparison.OrdinalIgnoreCase) &&
            r.Destination.Code.Equals(destination, StringComparison.OrdinalIgnoreCase));
    }
}
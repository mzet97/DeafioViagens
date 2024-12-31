using DesafioViagens.Domain.Entities;

namespace DesafioViagens.Domain.Services;

public class DijkstraRouteCalculator : IRouteCalculator
{
    public (int cost, List<Airport> path) CalculateBestRoute(
        IEnumerable<Route> routes,
        Airport origin,
        Airport destination
    )
    {
        var graph = BuildGraph(routes);

        var (distances, previous) = Dijkstra(graph, origin.Code);

        if (!distances.ContainsKey(destination.Code) || distances[destination.Code] == int.MaxValue)
        {
            return (int.MaxValue, new List<Airport>());
        }

        var pathCodes = ReconstructPath(previous, origin.Code, destination.Code);

        var pathAirports = pathCodes.Select(code => new Airport(code)).ToList();

        return (distances[destination.Code], pathAirports);
    }

    private Dictionary<string, List<(string, int)>> BuildGraph(IEnumerable<Route> routes)
    {
        var graph = new Dictionary<string, List<(string, int)>>();

        foreach (var route in routes)
        {
            var originCode = route.Origin.Code;
            var destinationCode = route.Destination.Code;
            var cost = route.Cost;

            if (!graph.ContainsKey(originCode))
                graph[originCode] = new List<(string, int)>();

            graph[originCode].Add((destinationCode, cost));

            if (!graph.ContainsKey(destinationCode))
                graph[destinationCode] = new List<(string, int)>();
        }

        return graph;
    }

    private (Dictionary<string, int> distances, Dictionary<string, string> previous)
        Dijkstra(Dictionary<string, List<(string, int)>> graph, string origin)
    {
        var distances = new Dictionary<string, int>();
        var previous = new Dictionary<string, string>();

        foreach (var node in graph.Keys)
        {
            distances[node] = int.MaxValue;
            previous[node] = null;
        }

        if (!distances.ContainsKey(origin))
        {
            distances[origin] = 0;
            previous[origin] = null;
        }
        else
        {
            distances[origin] = 0;
        }

        var pq = new PriorityQueue<string, int>();

        pq.Enqueue(origin, 0);

        while (pq.Count > 0)
        {
            pq.TryDequeue(out var current, out var currentDist);

            if (currentDist > distances[current])
                continue;

            if (!graph.ContainsKey(current))
                continue;

            foreach (var (neighbor, cost) in graph[current])
            {
                int alt = distances[current] + cost;
                if (alt < distances[neighbor])
                {
                    distances[neighbor] = alt;
                    previous[neighbor] = current;
                    pq.Enqueue(neighbor, alt);
                }
            }
        }

        return (distances, previous);
    }

    private List<string> ReconstructPath(Dictionary<string, string> previous, string origin, string destination)
    {
        var path = new List<string>();
        var current = destination;

        while (current != null)
        {
            path.Add(current);

            if (!previous.ContainsKey(current))
                break;

            current = previous[current];
        }

        path.Reverse();

        if (path.FirstOrDefault() != origin)
            return new List<string>();

        return path;
    }
}

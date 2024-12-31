using DesafioViagens.Domain.Entities;
using DesafioViagens.Domain.Services;

namespace DesafioViagens.Domain.Test;

public class DijkstraRouteCalculatorTests
{
    [Fact]
    public void CalculateBestRoute_ShouldReturnCheapestPath_WhenMultipleRoutesAreAvailable()
    {
        // ARRANGE
        var routes = new List<Route>
            {
                new Route(new Airport("GRU"), new Airport("BRC"), 10),
                new Route(new Airport("BRC"), new Airport("SCL"), 5),
                new Route(new Airport("GRU"), new Airport("CDG"), 75),
                new Route(new Airport("GRU"), new Airport("SCL"), 20),
                new Route(new Airport("GRU"), new Airport("ORL"), 56),
                new Route(new Airport("ORL"), new Airport("CDG"), 5),
                new Route(new Airport("SCL"), new Airport("ORL"), 20)
            };

        var calculator = new DijkstraRouteCalculator();

        // ACT
        var (cost, path) = calculator.CalculateBestRoute(routes, new Airport("GRU"), new Airport("CDG"));

        // ASSERT
        Assert.Equal(40, cost);
        var pathCodes = path.Select(a => a.Code).ToList();
        Assert.Equal(new[] { "GRU", "BRC", "SCL", "ORL", "CDG" }, pathCodes);
    }

    [Fact]
    public void CalculateBestRoute_ShouldReturnMaxValueAndEmptyPath_WhenNoRouteExists()
    {
        // ARRANGE
        var routes = new List<Route>
            {
                new Route(new Airport("AAA"), new Airport("BBB"), 10),
                new Route(new Airport("CCC"), new Airport("DDD"), 15)
            };

        var calculator = new DijkstraRouteCalculator();

        // ACT
        var (cost, path) = calculator.CalculateBestRoute(routes, new Airport("AAA"), new Airport("ZZZ"));

        // ASSERT
        Assert.Equal(int.MaxValue, cost);
        Assert.Empty(path);
    }

    [Fact]
    public void CalculateBestRoute_ShouldReturnDirectRoute_WhenItIsCheapest()
    {
        // ARRANGE
        var routes = new List<Route>
            {
                new Route(new Airport("A"), new Airport("B"), 10),
                new Route(new Airport("A"), new Airport("C"), 25),
                new Route(new Airport("C"), new Airport("B"), 30)
            };

        var calculator = new DijkstraRouteCalculator();

        // ACT
        var (cost, path) = calculator.CalculateBestRoute(routes, new Airport("A"), new Airport("B"));

        // ASSERT
        Assert.Equal(10, cost);
        var pathCodes = path.Select(a => a.Code).ToList();
        Assert.Equal(new[] { "A", "B" }, pathCodes);
    }
}

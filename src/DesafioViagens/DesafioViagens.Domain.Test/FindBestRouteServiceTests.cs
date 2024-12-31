using DesafioViagens.Domain.Entities;
using DesafioViagens.Domain.Repositories;
using DesafioViagens.Domain.Services;
using Moq;

namespace DesafioViagens.Domain.Test;

public class FindBestRouteServiceTests
{
    [Fact]
    public void FindBestRoute_ShouldReturnExpectedCostAndPath()
    {
        // ARRANGE
        var routeRepositoryMock = new Mock<IRouteRepository>();
        var routeCalculatorMock = new Mock<IRouteCalculator>();

        var fakeRoutes = new List<Route>
            {
                new Route(new Airport("GRU"), new Airport("BRC"), 10),
                new Route(new Airport("BRC"), new Airport("SCL"), 5),
                new Route(new Airport("GRU"), new Airport("CDG"), 75)
            };
        routeRepositoryMock
            .Setup(r => r.GetAllRoutes())
            .Returns(fakeRoutes);

        var expectedCost = 40;
        var expectedPath = new List<Airport>
            {
                new Airport("GRU"),
                new Airport("BRC"),
                new Airport("SCL"),
                new Airport("CDG")
            };
        routeCalculatorMock
            .Setup(c => c.CalculateBestRoute(
                It.IsAny<IEnumerable<Route>>(),
                It.IsAny<Airport>(),
                It.IsAny<Airport>()))
            .Returns((expectedCost, expectedPath));

        var service = new FindBestRouteService(routeRepositoryMock.Object, routeCalculatorMock.Object);

        // ACT
        var result = service.FindBestRoute("GRU", "CDG");

        // ASSERT
        Assert.Equal(expectedCost, result.Cost);
        Assert.Equal(expectedPath.Count, result.Path.Count);
        Assert.True(result.Path.Select(x => x.Code).SequenceEqual(expectedPath.Select(x => x.Code)));

        routeRepositoryMock.Verify(r => r.GetAllRoutes(), Times.Once);

        routeCalculatorMock.Verify(c => c.CalculateBestRoute(
            fakeRoutes,
            It.Is<Airport>(a => a.Code == "GRU"),
            It.Is<Airport>(a => a.Code == "CDG")),
            Times.Once
        );
    }

    [Fact]
    public void FindBestRoute_WhenNoPathExists_ShouldReturnMaxValueCost()
    {
        // ARRANGE
        var routeRepositoryMock = new Mock<IRouteRepository>();
        var routeCalculatorMock = new Mock<IRouteCalculator>();

        var fakeRoutes = new List<Route>
            {
                new Route(new Airport("AAA"), new Airport("BBB"), 10)
            };
        routeRepositoryMock
            .Setup(r => r.GetAllRoutes())
            .Returns(fakeRoutes);

        routeCalculatorMock
            .Setup(c => c.CalculateBestRoute(
                It.IsAny<IEnumerable<Route>>(),
                It.IsAny<Airport>(),
                It.IsAny<Airport>()))
            .Returns((int.MaxValue, new List<Airport>()));

        var service = new FindBestRouteService(routeRepositoryMock.Object, routeCalculatorMock.Object);

        // ACT
        var result = service.FindBestRoute("AAA", "XYZ");

        // ASSERT
        Assert.Equal(int.MaxValue, result.Cost);
        Assert.Empty(result.Path);

        routeRepositoryMock.Verify(r => r.GetAllRoutes(), Times.Once);
        routeCalculatorMock.Verify(c => c.CalculateBestRoute(fakeRoutes,
            It.Is<Airport>(a => a.Code == "AAA"),
            It.Is<Airport>(a => a.Code == "XYZ")),
            Times.Once
        );
    }
}
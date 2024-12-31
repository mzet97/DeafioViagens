namespace DesafioViagens.Domain.Entities;

public class Route
{
    public Airport Origin { get; private set; }
    public Airport Destination { get; private set; }
    public int Cost { get; private set; }

    public Route(Airport origin, Airport destination, int cost)
    {
        Origin = origin;
        Destination = destination;
        Cost = cost;
    }

    public override string ToString()
    {
        return $"{Origin.Code} -> {Destination.Code} = {Cost}";
    }
}


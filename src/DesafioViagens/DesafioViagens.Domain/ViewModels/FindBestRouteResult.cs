using DesafioViagens.Domain.Entities;

namespace DesafioViagens.Domain.ViewModels;

public class FindBestRouteResult
{
    public int Cost { get; set; }
    public List<Airport> Path { get; set; }
}
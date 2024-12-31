using DesafioViagens.Domain.ViewModels;

namespace DesafioViagens.Domain.Services;

public interface IFindBestRouteService
{
    FindBestRouteResult FindBestRoute(string originCode, string destinationCode);
}

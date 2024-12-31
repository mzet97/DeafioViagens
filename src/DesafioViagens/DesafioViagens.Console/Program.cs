using DesafioViagens.Domain.Repositories;
using DesafioViagens.Domain.Services;
using DesafioViagens.Infrastructure.Repositories;

namespace DesafioViagens;

class Program
{
    static void Main(string[] args)
    {
        string csvPath = args.FirstOrDefault() ?? "rotas.csv";
        IRouteRepository routeRepository = new CsvRouteRepository(csvPath);

        IRouteCalculator routeCalculator = new DijkstraRouteCalculator();

        IFindBestRouteService findBestRouteService = new FindBestRouteService(routeRepository, routeCalculator);

        while (true)
        {
            Console.Write("Digite a rota (Ex: GRU-CDG): ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                break;

            var parts = input.Split('-');
            if (parts.Length != 2)
            {
                Console.WriteLine("Formato inválido. Utilize Origem-Destino. Ex: GRU-CDG");
                continue;
            }

            var origin = parts[0].Trim().ToUpper();
            var destination = parts[1].Trim().ToUpper();

            var result = findBestRouteService.FindBestRoute(origin, destination);

            if (result.Cost == int.MaxValue || result.Path == null || !result.Path.Any())
            {
                Console.WriteLine($"Não existe caminho de {origin} para {destination}");
            }
            else
            {
                var pathStr = string.Join(" - ", result.Path.Select(a => a.Code));
                Console.WriteLine($"Melhor rota: {pathStr} ao custo de ${result.Cost}");
            }
        }
    }
}
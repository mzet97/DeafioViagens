using DesafioViagens.API.DTOs;
using DesafioViagens.Domain.Entities;
using DesafioViagens.Domain.Exceptions;
using DesafioViagens.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace DesafioViagens.API.Controllers;

[ApiController]
[Route("api/v1/routes")]
public class RoutesController : ControllerBase
{
    private readonly IFindBestRouteService _findBestRouteService;
    private readonly IRouteService _routeService;

    public RoutesController(
        IFindBestRouteService findBestRouteService,
        IRouteService routeService)
    {
        _findBestRouteService = findBestRouteService;
        _routeService = routeService;
    }

    [HttpGet("best")]
    public IActionResult GetBestRoute([FromQuery] string origin, [FromQuery] string destination)
    {
        if (string.IsNullOrWhiteSpace(origin) || string.IsNullOrWhiteSpace(destination))
            return BadRequest("Origem e destino são obrigatórios.");

        var result = _findBestRouteService.FindBestRoute(origin.ToUpper(), destination.ToUpper());

        if (result.Cost == int.MaxValue || result.Path == null || !result.Path.Any())
        {
            return NotFound($"Não existe caminho de {origin} para {destination}");
        }

        var response = new BestRouteResponseDto
        {
            Cost = result.Cost,
            Path = result.Path.Select(a => a.Code).ToList()
        };

        return Ok(response);
    }

    [HttpGet]
    public IActionResult GetAllRoutes()
    {
        var routes = _routeService.GetAllRoutes();

        var response = routes.Select(r => new
        {
            Origin = r.Origin.Code,
            Destination = r.Destination.Code,
            Cost = r.Cost
        });
        return Ok(response);
    }

    [HttpGet("{origin}/{destination}")]
    public IActionResult GetRouteByKeys(string origin, string destination)
    {
        try
        {
            var route = _routeService.GetRoute(origin.ToUpper(), destination.ToUpper());

            var response = new
            {
                Origin = route.Origin.Code,
                Destination = route.Destination.Code,
                Cost = route.Cost
            };
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }


    [HttpPost]
    public IActionResult CreateRoute([FromBody] CreateRouteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var route = new Domain.Entities.Route(
                new Airport(dto.Origin.ToUpper()),
                new Airport(dto.Destination.ToUpper()),
                dto.Cost
            );

            _routeService.CreateRoute(route);

            return CreatedAtAction(nameof(GetRouteByKeys),
                new { origin = dto.Origin, destination = dto.Destination },
                dto);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{origin}/{destination}")]
    public IActionResult UpdateRoute(string origin, string destination, [FromBody] UpdateRouteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updated = new Domain.Entities.Route(
                new Airport(dto.Origin.ToUpper()),
                new Airport(dto.Destination.ToUpper()),
                dto.Cost
            );

            bool updatedSuccess = _routeService.UpdateRoute(origin.ToUpper(), destination.ToUpper(), updated);

            if (!updatedSuccess)
            {
                return Ok("No changes were made (the route is the same).");
            }

            return Ok("Route updated successfully.");
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{origin}/{destination}")]
    public IActionResult DeleteRoute(string origin, string destination)
    {
        try
        {
            var deleted = _routeService.DeleteRoute(origin.ToUpper(), destination.ToUpper());
            if (!deleted)
            {
                return NotFound($"Rota não encontrada: {origin} -> {destination}");
            }
            return Ok("Rota deletada com sucesso.");
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
namespace DesafioViagens.API.DTOs;

public class CreateRouteDto
{
    public string Origin { get; set; }
    public string Destination { get; set; }
    public int Cost { get; set; }
}

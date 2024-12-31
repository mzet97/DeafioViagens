namespace DesafioViagens.Domain.Entities;

public class Airport
{
    public string Code { get; private set; }

    public Airport(string code)
    {
        Code = code;
    }
}

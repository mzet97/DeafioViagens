using DesafioViagens.API.Config;
using DesafioViagens.Domain.Repositories;
using DesafioViagens.Infrastructure.Repositories;
using DesafioViagens.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IRouteRepository>(sp =>
    new CsvRouteRepository(args.FirstOrDefault() ?? "rotas.csv")
);

builder.Services.ResolveDependenciesDomain();
builder.Services.AddCorsConfig();
builder.Services.AddSwaggerConfig();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();
app.UseSwaggerConfig();

app.Run();

/*
 * Punto de entrada del microservicio PDF.
 *
 * Responsabilidades:
 * - Configurar controladores REST.
 * - Habilitar Swagger para pruebas y documentación visual.
 * - Registrar la capa Application.
 * - Registrar la capa Infrastructure, que contiene EF Core y QuestPDF.
 * - Configurar HttpClient con políticas de resiliencia usando Polly.
 *
 * Nota arquitectónica:
 * La API referencia Infrastructure únicamente para registrar dependencias.
 * La lógica de generación de PDF vive en Application e Infrastructure.
 */

using PDF.Api;
using PDF.Application;
using PDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddHttpClient("ExternalServices")
    .AddPolicyHandler(PollyPolicies.GetRetryPolicy())
    .AddPolicyHandler(PollyPolicies.GetCircuitBreakerPolicy())
    .AddPolicyHandler(PollyPolicies.GetTimeoutPolicy());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

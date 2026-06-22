/*
 * Punto de entrada del microservicio Agenda.
 *
 * Responsabilidades:
 * - Configurar la capa REST.
 * - Habilitar Swagger.
 * - Registrar los servicios de Application.
 * - Registrar las implementaciones de Infrastructure.
 * - Configurar HttpClient con políticas de resiliencia usando Polly.
 *
 * Nota arquitectónica:
 * La API actúa como composition root. Por eso referencia Infrastructure
 * únicamente para registrar dependencias, no para usar DbContext,
 * repositorios o lógica de datos directamente.
 */

using Agenda.Api;
using Agenda.Application;
using Agenda.Infrastructure;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Agenda Microservice API",
        Version = "v1",
        Description = "Microservicio para generación optimizada de agendas de inversionistas."
    });

    var xmlFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

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

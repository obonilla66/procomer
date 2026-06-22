
using Agenda.Application.Dtos;
using Agenda.Application.Interfaces;
using Agenda.Infrastructure.Clients;
using Agenda.Infrastructure.Persistence;
using Agenda.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Agenda.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AgendaDb")
            ?? configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string AgendaDb o DefaultConnection es requerida.");

        services.AddDbContext<AgendaDbContext>(options =>
        {
            options.UseSqlServer(
                connectionString,
                sqlOptions => sqlOptions.EnableRetryOnFailure());
        });

        var catalogApiUrl = configuration["Services:CatalogApi"];

        if (string.IsNullOrWhiteSpace(catalogApiUrl))
        {
            throw new InvalidOperationException("La configuración Services:CatalogApi es requerida.");
        }

        services.AddHttpClient<IInversorRepository, CatalogInversorClientRepository>(client =>
        {
            client.BaseAddress = new Uri(catalogApiUrl);
        })
        .AddPolicyHandler(GetRetryPolicy())
        .AddPolicyHandler(GetCircuitBreakerPolicy())
        .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(10));

        services.AddHttpClient<IParticipanteRepository, CatalogParticipanteClientRepository>(client =>
        {
            client.BaseAddress = new Uri(catalogApiUrl);
        })
        .AddPolicyHandler(GetRetryPolicy())
        .AddPolicyHandler(GetCircuitBreakerPolicy())
        .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(10));

        services.AddHttpClient<IMatrizTrasladoRepository, CatalogMatrizTrasladoClientRepository>(client =>
        {
            client.BaseAddress = new Uri(catalogApiUrl);
        })
        .AddPolicyHandler(GetRetryPolicy())
        .AddPolicyHandler(GetCircuitBreakerPolicy())
        .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(10));

        var pdfApiUrl = configuration["Services:PdfApi"];

        if (!string.IsNullOrWhiteSpace(pdfApiUrl))
        {
            services.AddHttpClient<IPdfClient, PdfClient>(client =>
            {
                client.BaseAddress = new Uri(pdfApiUrl);
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy())
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(10));
        }
        else
        {
            services.AddScoped<IPdfClient, MissingPdfClient>();
        }

        services.AddScoped<IAgendaRepository, AgendaRepository>();

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30));
    }

    private sealed class MissingPdfClient : IPdfClient
    {
        public Task<PdfFileResponse> ObtenerPdfAgendaAsync(
            int idAgenda,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new PdfFileResponse
            {
                Exitoso = false,
                Mensaje = "Services:PdfApi no está configurado."
            });
        }
    }
}

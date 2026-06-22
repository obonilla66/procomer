/*
 * Registro de servicios propios de la capa Application.
 *
 * Aquí se registran los casos de uso del microservicio.
 * Application no conoce Entity Framework, SQL Server ni QuestPDF.
 */

using Microsoft.Extensions.DependencyInjection;
using PDF.Application.Services;

namespace PDF.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPdfAgendaService, PdfAgendaService>();
        return services;
    }
}

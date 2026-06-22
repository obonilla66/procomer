/*
 * Registro de servicios de la capa Application.
 *
 * Aquí se registran los casos de uso del microservicio.
 *
 * Application contiene la orquestación del scheduling, pero no conoce
 * Entity Framework, SQL Server ni detalles de persistencia.
 */

using Agenda.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Agenda.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAgendaSchedulingService, AgendaSchedulingService>();
        return services;
    }
}

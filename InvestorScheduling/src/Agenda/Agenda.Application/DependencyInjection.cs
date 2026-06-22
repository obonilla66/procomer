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

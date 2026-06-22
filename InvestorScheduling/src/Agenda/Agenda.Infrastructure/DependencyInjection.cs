using Agenda.Application.Interfaces;
using Agenda.Infrastructure.Persistence;
using Agenda.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Agenda.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AgendaDb");

        services.AddDbContext<AgendaDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IInversorRepository, InversorRepository>();
        services.AddScoped<IParticipanteRepository, ParticipanteRepository>();
        services.AddScoped<IMatrizTrasladoRepository, MatrizTrasladoRepository>();
        services.AddScoped<IAgendaRepository, AgendaRepository>();

        return services;
    }
}

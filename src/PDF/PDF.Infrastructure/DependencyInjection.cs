/*
 * Registro de implementaciones concretas de Infrastructure.
 *
 * Aquí se configura:
 * - DbContext con SQL Server.
 * - Repositorio que consulta la agenda.
 * - Generador de PDF basado en QuestPDF.
 *
 * La API llama este método durante el arranque, pero no usa directamente
 * DbContext ni repositorios.
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PDF.Application.Interfaces;
using PDF.Infrastructure.Pdf;
using PDF.Infrastructure.Persistence;
using PDF.Infrastructure.Repositories;

namespace PDF.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<PdfOptions>(configuration.GetSection("Pdf"));

        services.AddDbContext<PdfDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AgendaDb") ?? configuration.GetConnectionString("DefaultConnection") ?? configuration.GetConnectionString("PdfDb")));

        services.AddScoped<IAgendaPdfRepository, AgendaPdfRepository>();
        services.AddScoped<IPdfGenerator, AgendaPdfGenerator>();

        return services;
    }
}

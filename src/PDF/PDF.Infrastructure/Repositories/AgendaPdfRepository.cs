/*
 * Implementación concreta para leer la agenda desde SQL Server.
 *
 * Responsabilidades:
 * - Consultar la agenda por IdAgenda.
 * - Incluir inversor, detalles, participante, oficina e idioma.
 * - Mapear entidades EF a un modelo de dominio AgendaPdfData.
 *
 * Esta clase mantiene EF Core aislado dentro de Infrastructure.
 */

using Microsoft.EntityFrameworkCore;
using PDF.Application.Interfaces;
using PDF.Domain.Models;
using PDF.Infrastructure.Persistence;

namespace PDF.Infrastructure.Repositories;

public class AgendaPdfRepository : IAgendaPdfRepository
{
    private readonly PdfDbContext _context;

    public AgendaPdfRepository(PdfDbContext context)
    {
        _context = context;
    }

    public async Task<AgendaPdfData?> ObtenerAgendaConfirmadaAsync(
        int idAgenda,
        CancellationToken cancellationToken)
    {
        var agenda = await _context.Agendas
            .AsNoTracking()
            .Include(x => x.Inversor)
            .Include(x => x.Detalles)
                .ThenInclude(x => x.Participante)
                    .ThenInclude(x => x.Oficina)
            .Include(x => x.Detalles)
                .ThenInclude(x => x.Idioma)
            .FirstOrDefaultAsync(x => x.Id == idAgenda, cancellationToken);

        if (agenda is null)
            return null;

        return new AgendaPdfData
        {
            IdAgenda = agenda.Id,
            Fecha = agenda.Fecha,
            Estado = agenda.Estado,
            NombreInversor = agenda.Inversor.NombreCompleto,
            EmpresaRepresenta = agenda.Inversor.EmpresaRepresenta,
            PaisOrigen = agenda.Inversor.PaisOrigen,
            Detalles = agenda.Detalles
                .OrderBy(x => x.FechaHoraInicio)
                .Select(x => new AgendaPdfDetalle
                {
                    FechaHoraInicio = x.FechaHoraInicio,
                    FechaHoraFin = x.FechaHoraFin,
                    NombreParticipante = x.Participante.NombreCompleto,
                    Cargo = x.Participante.Cargo,
                    Institucion = x.Participante.Institucion,
                    Oficina = x.Participante.Oficina.Nombre,
                    Direccion = x.Participante.Oficina.Direccion,
                    CodigoIdioma = x.CodigoIdioma,
                    Idioma = x.Idioma.Idioma,
                    TiempoTrasladoSiguienteOficinaMinutos = x.TiempoTrasladoSiguienteOficinaMinutos
                })
                .ToList()
        };
    }
}

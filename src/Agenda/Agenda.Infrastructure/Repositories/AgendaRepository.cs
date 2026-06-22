
using Agenda.Application.Dtos;
using Agenda.Application.Interfaces;
using Agenda.Domain.Entities;
using Agenda.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Infrastructure.Repositories;

public class AgendaRepository : IAgendaRepository
{
    private readonly AgendaDbContext _context;

    public AgendaRepository(AgendaDbContext context)
    {
        _context = context;
    }

    public async Task<int> CrearAgendaAsync(
        AgendaEntity agenda,
        IReadOnlyCollection<int> participantesCandidatos,
        IReadOnlyCollection<AgendaDetalle> detalles,
        CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        _context.Agendas.Add(agenda);
        await _context.SaveChangesAsync(cancellationToken);

        foreach (var idParticipante in participantesCandidatos.Distinct())
        {
            _context.AgendaParticipantes.Add(new AgendaParticipante
            {
                IdAgenda = agenda.Id,
                IdParticipante = idParticipante
            });
        }

        foreach (var detalle in detalles)
        {
            detalle.IdAgenda = agenda.Id;
            _context.AgendaDetalles.Add(detalle);
        }

        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return agenda.Id;
    }

    public async Task<List<AgendaResumenResponse>> ListarAsync(
        int? idInversor,
        DateTime? fecha,
        bool? estado,
        CancellationToken cancellationToken)
    {
        var query = _context.Agendas
            .AsNoTracking()
            .Include(x => x.Detalles)
            .AsQueryable();

        if (idInversor.HasValue)
        {
            query = query.Where(x => x.IdInversor == idInversor.Value);
        }

        if (fecha.HasValue)
        {
            var dia = fecha.Value.Date;
            query = query.Where(x => x.Fecha == dia);
        }

        if (estado.HasValue)
        {
            query = query.Where(x => x.Estado == estado.Value);
        }

        return await query
            .OrderByDescending(x => x.Fecha)
            .ThenByDescending(x => x.Id)
            .Select(x => new AgendaResumenResponse
            {
                Id = x.Id,
                IdInversor = x.IdInversor,
                Fecha = x.Fecha,
                DuracionReunionMinutos = x.DuracionReunionMinutos,
                CantidadReunionesMeta = x.CantidadReunionesMeta,
                Estado = x.Estado,
                CantidadReunionesProgramadas = x.Detalles.Count
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<AgendaDetalleCompletaResponse?> ObtenerDetalleAsync(
        int idAgenda,
        CancellationToken cancellationToken)
    {
        var agenda = await _context.Agendas
            .AsNoTracking()
            .Include(x => x.Detalles)
                .ThenInclude(x => x.Participante)
                    .ThenInclude(x => x.Oficina)
            .FirstOrDefaultAsync(x => x.Id == idAgenda, cancellationToken);

        if (agenda is null)
        {
            return null;
        }

        return new AgendaDetalleCompletaResponse
        {
            Id = agenda.Id,
            IdInversor = agenda.IdInversor,
            Fecha = agenda.Fecha,
            DuracionReunionMinutos = agenda.DuracionReunionMinutos,
            CantidadReunionesMeta = agenda.CantidadReunionesMeta,
            Estado = agenda.Estado,
            Detalles = agenda.Detalles
                .OrderBy(x => x.FechaHoraInicio)
                .Select(x => new AgendaDetalleResponse
                {
                    IdParticipante = x.IdParticipante,
                    Participante = x.Participante?.NombreCompleto ?? string.Empty,
                    IdOficina = x.Participante?.IdOficina ?? 0,
                    Oficina = x.Participante?.Oficina?.Nombre ?? string.Empty,
                    CodigoIdioma = x.CodigoIdioma,
                    FechaHoraInicio = x.FechaHoraInicio,
                    FechaHoraFin = x.FechaHoraFin,
                    TiempoTrasladoSiguienteOficinaMinutos = x.TiempoTrasladoSiguienteOficinaMinutos
                })
                .ToList()
        };
    }

    public async Task<bool> AnularAsync(
        int idAgenda,
        CancellationToken cancellationToken)
    {
        var agenda = await _context.Agendas
            .FirstOrDefaultAsync(x => x.Id == idAgenda, cancellationToken);

        if (agenda is null)
        {
            return false;
        }

        agenda.Estado = false;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}

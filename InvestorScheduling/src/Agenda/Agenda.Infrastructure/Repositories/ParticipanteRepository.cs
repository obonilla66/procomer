using Agenda.Application.Interfaces;
using Agenda.Domain.Entities;
using Agenda.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Infrastructure.Repositories;

public class ParticipanteRepository : IParticipanteRepository
{
    private readonly AgendaDbContext _context;

    public ParticipanteRepository(AgendaDbContext context)
    {
        _context = context;
    }

    public Task<List<Participante>> ObtenerCandidatosConDisponibilidadAsync(
        IReadOnlyCollection<int> idsParticipantes,
        DateTime fecha,
        CancellationToken cancellationToken)
    {
        var dia = fecha.Date;
        var diaSiguiente = dia.AddDays(1);

        return _context.Participantes
            .Include(x => x.Oficina)
            .Include(x => x.Idiomas)
            .Include(x => x.Horarios.Where(h =>
                h.FechaHoraInicio < diaSiguiente &&
                h.FechaHoraFin > dia))
            .Where(x =>
                x.Estado &&
                idsParticipantes.Contains(x.Id) &&
                x.Horarios.Any(h =>
                    h.FechaHoraInicio < diaSiguiente &&
                    h.FechaHoraFin > dia))
            .ToListAsync(cancellationToken);
    }
}

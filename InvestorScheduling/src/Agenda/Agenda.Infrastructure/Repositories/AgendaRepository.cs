using Agenda.Application.Interfaces;
using Agenda.Domain.Entities;
using Agenda.Infrastructure.Persistence;

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
}

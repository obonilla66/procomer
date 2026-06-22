using Agenda.Domain.Entities;

namespace Agenda.Application.Interfaces;

public interface IAgendaRepository
{
    Task<int> CrearAgendaAsync(
        AgendaEntity agenda,
        IReadOnlyCollection<int> participantesCandidatos,
        IReadOnlyCollection<AgendaDetalle> detalles,
        CancellationToken cancellationToken);
}

using Agenda.Domain.Entities;

namespace Agenda.Application.Interfaces;

public interface IParticipanteRepository
{
    Task<List<Participante>> ObtenerCandidatosConDisponibilidadAsync(
        IReadOnlyCollection<int> idsParticipantes,
        DateTime fecha,
        CancellationToken cancellationToken);
}

/*
 * Contrato para consultar participantes candidatos.
 *
 * Debe devolver participantes activos, con oficina, idiomas y disponibilidad
 * para la fecha solicitada.
 */

using Agenda.Domain.Entities;

namespace Agenda.Application.Interfaces;

public interface IParticipanteRepository
{
    Task<List<Participante>> ObtenerCandidatosConDisponibilidadAsync(
        IReadOnlyCollection<int> idsParticipantes,
        DateTime fecha,
        CancellationToken cancellationToken);
}

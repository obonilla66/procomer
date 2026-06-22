/*
 * Contrato para persistir la agenda generada.
 *
 * Incluye:
 * - Encabezado de Agenda.
 * - Participantes candidatos.
 * - Detalle de reuniones efectivamente programadas.
 */

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

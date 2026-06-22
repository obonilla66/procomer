/*
 * Contrato del caso de uso de scheduling.
 *
 * La API depende de esta interfaz para solicitar la generación de agenda
 * sin conocer la implementación concreta.
 */

using Agenda.Application.Dtos;

namespace Agenda.Application.Services;

public interface IAgendaSchedulingService
{
    Task<AgendaGeneradaResponse> GenerarAsync(
        GenerarAgendaRequest request,
        CancellationToken cancellationToken);
}

/*
 * Contrato del caso de uso de generación de PDF de agenda.
 *
 * La API depende de esta interfaz para no acoplarse a la implementación.
 */

using PDF.Application.Dtos;

namespace PDF.Application.Services;

public interface IPdfAgendaService
{
    Task<PdfResponse> GenerarAgendaPdfAsync(
        int idAgenda,
        CancellationToken cancellationToken);
}

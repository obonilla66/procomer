/*
 * Contrato de lectura de datos para el PDF.
 *
 * Application define lo que necesita:
 * una agenda confirmada con inversor, reuniones, participantes, oficinas
 * e idioma.
 *
 * Infrastructure decide cómo obtener esos datos.
 */

using PDF.Domain.Models;

namespace PDF.Application.Interfaces;

public interface IAgendaPdfRepository
{
    Task<AgendaPdfData?> ObtenerAgendaConfirmadaAsync(
        int idAgenda,
        CancellationToken cancellationToken);
}

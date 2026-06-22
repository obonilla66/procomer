
using Agenda.Application.Dtos;

namespace Agenda.Application.Interfaces;

public interface IPdfClient
{
    Task<PdfFileResponse> ObtenerPdfAgendaAsync(
        int idAgenda,
        CancellationToken cancellationToken);
}


using Agenda.Application.Dtos;
using Agenda.Application.Interfaces;

namespace Agenda.Infrastructure.Clients;

public class PdfClient : IPdfClient
{
    private readonly HttpClient _httpClient;

    public PdfClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PdfFileResponse> ObtenerPdfAgendaAsync(
        int idAgenda,
        CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"/api/pdf/agendas/{idAgenda}", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadAsStringAsync(cancellationToken);

            return new PdfFileResponse
            {
                Exitoso = false,
                Mensaje = string.IsNullOrWhiteSpace(message)
                    ? "No fue posible generar el PDF de la agenda."
                    : message
            };
        }

        var content = await response.Content.ReadAsByteArrayAsync(cancellationToken);

        var contentType = response.Content.Headers.ContentType?.ToString()
            ?? "application/pdf";

        var fileName = response.Content.Headers.ContentDisposition?.FileNameStar
            ?? response.Content.Headers.ContentDisposition?.FileName?.Trim('"')
            ?? $"agenda-{idAgenda}.pdf";

        return new PdfFileResponse
        {
            Exitoso = true,
            Mensaje = "PDF obtenido correctamente.",
            FileName = fileName,
            ContentType = contentType,
            Content = content
        };
    }
}

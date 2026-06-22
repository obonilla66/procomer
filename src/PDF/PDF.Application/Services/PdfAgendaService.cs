/*
 * Caso de uso principal del microservicio PDF.
 *
 * Flujo:
 * 1. Recibe el IdAgenda.
 * 2. Valida que el parámetro sea correcto.
 * 3. Solicita a un repositorio la información consolidada de la agenda.
 * 4. Verifica reglas de negocio: agenda confirmada y con reuniones.
 * 5. Solicita a IPdfGenerator la creación binaria del documento PDF.
 * 6. Devuelve un PdfResponse listo para que la API lo entregue como archivo.
 *
 * Esta clase orquesta el proceso, pero no conoce detalles de base de datos
 * ni detalles técnicos de la librería de PDF.
 */

using PDF.Application.Dtos;
using PDF.Application.Interfaces;
using PDF.Domain.Rules;

namespace PDF.Application.Services;

public class PdfAgendaService : IPdfAgendaService
{
    private readonly IAgendaPdfRepository _agendaRepository;
    private readonly IPdfGenerator _pdfGenerator;

    public PdfAgendaService(
        IAgendaPdfRepository agendaRepository,
        IPdfGenerator pdfGenerator)
    {
        _agendaRepository = agendaRepository;
        _pdfGenerator = pdfGenerator;
    }

    public async Task<PdfResponse> GenerarAgendaPdfAsync(
        int idAgenda,
        CancellationToken cancellationToken)
    {
        if (idAgenda <= 0)
            return Error("Debe indicar un IdAgenda válido.");

        var agenda = await _agendaRepository.ObtenerAgendaConfirmadaAsync(
            idAgenda,
            cancellationToken);

        if (agenda is null)
            return Error("La agenda no existe.");

        if (!PdfBusinessRules.EsAgendaConfirmada(agenda))
            return Error("La agenda no está confirmada o no contiene reuniones programadas.");

        var bytes = _pdfGenerator.GenerarAgendaPdf(agenda);

        return new PdfResponse
        {
            Exitoso = true,
            Mensaje = "PDF generado exitosamente.",
            FileName = $"agenda-inversor-{agenda.IdAgenda}.pdf",
            ContentType = "application/pdf",
            Content = bytes
        };
    }

    private static PdfResponse Error(string mensaje)
    {
        return new PdfResponse
        {
            Exitoso = false,
            Mensaje = mensaje
        };
    }
}

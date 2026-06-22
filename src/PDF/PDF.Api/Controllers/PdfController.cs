/*
 * Controlador REST para la generación de documentos PDF.
 *
 * Expone un endpoint simple:
 *
 *   GET /api/pdf/agendas/{idAgenda}
 *
 * El controlador no genera el PDF directamente.
 * Solo recibe la solicitud, delega en Application y devuelve:
 * - 200 con application/pdf si la generación fue exitosa.
 * - 400 con mensaje funcional si la agenda no existe, no está confirmada
 *   o no contiene reuniones.
 */

using Microsoft.AspNetCore.Mvc;
using PDF.Application.Services;

namespace PDF.Api.Controllers;

[ApiController]
[Route("api/pdf")]
[Produces("application/pdf", "application/json")]
public class PdfController : ControllerBase
{
    private readonly IPdfAgendaService _pdfAgendaService;

    public PdfController(IPdfAgendaService pdfAgendaService)
    {
        _pdfAgendaService = pdfAgendaService;
    }

    /// <summary>
    /// Genera el PDF profesional de una agenda confirmada.
    /// </summary>
    /// <param name="idAgenda">Identificador de la agenda confirmada.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Archivo PDF listo para imprimir o enviar por correo electrónico.</returns>
    [HttpGet("agendas/{idAgenda:int}")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerarAgendaPdf(
        int idAgenda,
        CancellationToken cancellationToken)
    {
        var result = await _pdfAgendaService.GenerarAgendaPdfAsync(
            idAgenda,
            cancellationToken);

        if (!result.Exitoso)
        {
            return BadRequest(new
            {
                result.Mensaje
            });
        }

        return File(
            result.Content,
            result.ContentType,
            result.FileName);
    }
}

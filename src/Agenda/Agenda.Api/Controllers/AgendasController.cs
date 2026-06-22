
using Agenda.Application.Dtos;
using Agenda.Application.Interfaces;
using Agenda.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.Api.Controllers;

[ApiController]
[Route("agendas")]
[Route("api/agendas")]
[Produces("application/json")]
public class AgendasController : ControllerBase
{
    private readonly IAgendaSchedulingService _agendaSchedulingService;
    private readonly IAgendaRepository _agendaRepository;
    private readonly IPdfClient _pdfClient;

    public AgendasController(
        IAgendaSchedulingService agendaSchedulingService,
        IAgendaRepository agendaRepository,
        IPdfClient pdfClient)
    {
        _agendaSchedulingService = agendaSchedulingService;
        _agendaRepository = agendaRepository;
        _pdfClient = pdfClient;
    }

    /// <summary>
    /// Genera una agenda optimizada para un inversor en una fecha específica.
    /// </summary>
    [HttpPost("generar")]
    [ProducesResponseType(typeof(AgendaGeneradaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AgendaGeneradaResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AgendaGeneradaResponse>> Generar(
        [FromBody] GenerarAgendaRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _agendaSchedulingService.GenerarAsync(request, cancellationToken);

        if (!result.Exitoso)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Lista agendas existentes con filtros opcionales por inversor, fecha y estado.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<AgendaResumenResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AgendaResumenResponse>>> GetAll(
        [FromQuery] int? idInversor,
        [FromQuery] DateTime? fecha,
        [FromQuery] bool? estado,
        CancellationToken cancellationToken)
    {
        var result = await _agendaRepository.ListarAsync(
            idInversor,
            fecha,
            estado,
            cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Retorna el detalle completo de una agenda.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AgendaDetalleCompletaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AgendaDetalleCompletaResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _agendaRepository.ObtenerDetalleAsync(id, cancellationToken);

        if (result is null)
        {
            return NotFound(new { mensaje = "La agenda no existe." });
        }

        return Ok(result);
    }

    /// <summary>
    /// Realiza la anulación lógica de la agenda.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Anular(
        int id,
        CancellationToken cancellationToken)
    {
        var anulada = await _agendaRepository.AnularAsync(id, cancellationToken);

        if (!anulada)
        {
            return NotFound(new { mensaje = "La agenda no existe." });
        }

        return NoContent();
    }

    /// <summary>
    /// Devuelve el archivo PDF asociado a la agenda.
    /// </summary>
    [HttpGet("{id:int}/pdf")]
    [Produces("application/pdf", "application/json")]
    public async Task<IActionResult> GetPdf(
        int id,
        CancellationToken cancellationToken)
    {
        var pdf = await _pdfClient.ObtenerPdfAgendaAsync(id, cancellationToken);

        if (!pdf.Exitoso)
        {
            return BadRequest(new { mensaje = pdf.Mensaje });
        }

        return File(pdf.Content, pdf.ContentType, pdf.FileName);
    }
}

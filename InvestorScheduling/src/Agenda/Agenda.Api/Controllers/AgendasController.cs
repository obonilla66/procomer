using Agenda.Application.Dtos;
using Agenda.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.Api.Controllers;

[ApiController]
[Route("api/agendas")]
[Produces("application/json")]
public class AgendasController : ControllerBase
{
    private readonly IAgendaSchedulingService _agendaSchedulingService;

    public AgendasController(IAgendaSchedulingService agendaSchedulingService)
    {
        _agendaSchedulingService = agendaSchedulingService;
    }

    /// <summary>
    /// Genera una agenda optimizada para un inversor en una fecha específica.
    /// </summary>
    /// <remarks>
    /// Reglas consideradas:
    /// - Fecha dentro del período de visita del inversor.
    /// - Participantes activos.
    /// - Idioma compartido entre inversor y participante.
    /// - Horario laboral de 08:00 a 17:00.
    /// - Almuerzo bloqueado de 12:00 a 13:00.
    /// - Tiempo de traslado suficiente entre oficinas.
    /// - Maximización de reuniones y minimización de traslados.
    /// </remarks>
    [HttpPost("generar")]
    [ProducesResponseType(typeof(AgendaGeneradaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AgendaGeneradaResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AgendaGeneradaResponse>> Generar(
        [FromBody] GenerarAgendaRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _agendaSchedulingService.GenerarAsync(request, cancellationToken);

        if (!result.Exitoso)
            return BadRequest(result);

        return Ok(result);
    }
}

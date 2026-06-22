using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/participante-horarios")]
public class ParticipanteHorariosController : ControllerBase
{
    private readonly CatalogDbContext _dbContext;

    public ParticipanteHorariosController(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<ParticipanteHorarioResponse>>> Get(
        [FromQuery] DateTime? fecha,
        [FromQuery] string? idsParticipantes)
    {
        var query = _dbContext.ParticipanteHorarios.AsQueryable();

        if (fecha.HasValue)
        {
            var inicio = fecha.Value.Date;
            var fin = inicio.AddDays(1);

            query = query.Where(x => x.FechaHoraInicio >= inicio && x.FechaHoraInicio < fin);
        }

        if (!string.IsNullOrWhiteSpace(idsParticipantes))
        {
            var ids = idsParticipantes
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            query = query.Where(x => ids.Contains(x.IdParticipante));
        }

        var horarios = await query
            .OrderBy(x => x.FechaHoraInicio)
            .Select(x => new ParticipanteHorarioResponse
            {
                Id = x.Id,
                IdParticipante = x.IdParticipante,
                FechaHoraInicio = x.FechaHoraInicio,
                FechaHoraFin = x.FechaHoraFin
            })
            .ToListAsync();

        return Ok(horarios);
    }

    [HttpPost]
    public async Task<ActionResult<ParticipanteHorarioResponse>> Create(CrearParticipanteHorarioRequest request)
    {
        if (!await _dbContext.Participantes.AnyAsync(x => x.Id == request.IdParticipante))
        {
            return BadRequest("El participante indicado no existe.");
        }

        try
        {
            var horario = new ParticipanteHorario(
                request.IdParticipante,
                request.FechaHoraInicio,
                request.FechaHoraFin);

            _dbContext.ParticipanteHorarios.Add(horario);
            await _dbContext.SaveChangesAsync();

            return Ok(new ParticipanteHorarioResponse
            {
                Id = horario.Id,
                IdParticipante = horario.IdParticipante,
                FechaHoraInicio = horario.FechaHoraInicio,
                FechaHoraFin = horario.FechaHoraFin
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

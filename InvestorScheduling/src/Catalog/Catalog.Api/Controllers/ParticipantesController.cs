using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/participantes")]
public class ParticipantesController : ControllerBase
{
    private readonly CatalogDbContext _dbContext;

    public ParticipantesController(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<ParticipanteResponse>>> GetAll([FromQuery] string? ids)
    {
        var query = _dbContext.Participantes
            .Include(x => x.Oficina)
            .Include(x => x.Idiomas)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(ids))
        {
            var idsParticipantes = ids
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            query = query.Where(x => idsParticipantes.Contains(x.Id));
        }

        var participantes = await query
            .OrderBy(x => x.NombreCompleto)
            .Select(x => new ParticipanteResponse
            {
                Id = x.Id,
                NombreCompleto = x.NombreCompleto,
                Cargo = x.Cargo,
                Institucion = x.Institucion,
                IdOficina = x.IdOficina,
                NombreOficina = x.Oficina != null ? x.Oficina.Nombre : string.Empty,
                DireccionOficina = x.Oficina != null ? x.Oficina.Direccion : string.Empty,
                Estado = x.Estado,
                CodigosIdioma = x.Idiomas.Select(i => i.CodigoIdioma).ToList()
            })
            .ToListAsync();

        return Ok(participantes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ParticipanteResponse>> GetById(int id)
    {
        var participante = await _dbContext.Participantes
            .Include(x => x.Oficina)
            .Include(x => x.Idiomas)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (participante is null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(participante));
    }

    [HttpPost]
    public async Task<ActionResult<ParticipanteResponse>> Create(CrearParticipanteRequest request)
    {
        var codigosIdioma = request.CodigosIdioma
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim().ToLowerInvariant())
            .Distinct()
            .ToList();

        if (!await _dbContext.Oficinas.AnyAsync(x => x.Id == request.IdOficina))
        {
            return BadRequest("La oficina indicada no existe.");
        }

        if (codigosIdioma.Count == 0)
        {
            return BadRequest("Debe indicar al menos un idioma para el participante.");
        }

        var idiomasValidos = await _dbContext.Idiomas
            .Where(x => codigosIdioma.Contains(x.CodigoIdioma))
            .Select(x => x.CodigoIdioma)
            .ToListAsync();

        if (idiomasValidos.Count != codigosIdioma.Count)
        {
            return BadRequest("Uno o más códigos de idioma no existen.");
        }

        try
        {
            var participante = new Participante(
                request.NombreCompleto,
                request.Cargo,
                request.Institucion,
                request.IdOficina,
                codigosIdioma,
                request.Estado);

            _dbContext.Participantes.Add(participante);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = participante.Id }, MapToResponse(participante));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private static ParticipanteResponse MapToResponse(Participante participante)
    {
        return new ParticipanteResponse
        {
            Id = participante.Id,
            NombreCompleto = participante.NombreCompleto,
            Cargo = participante.Cargo,
            Institucion = participante.Institucion,
            IdOficina = participante.IdOficina,
            NombreOficina = participante.Oficina?.Nombre ?? string.Empty,
            DireccionOficina = participante.Oficina?.Direccion ?? string.Empty,
            Estado = participante.Estado,
            CodigosIdioma = participante.Idiomas.Select(i => i.CodigoIdioma).ToList()
        };
    }
}

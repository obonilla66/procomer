using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/inversores")]
public class InversoresController : ControllerBase
{
    private readonly CatalogDbContext _dbContext;

    public InversoresController(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<InversorResponse>>> GetAll()
    {
        var inversores = await _dbContext.Inversores
            .Include(x => x.Idiomas)
            .OrderBy(x => x.NombreCompleto)
            .Select(x => new InversorResponse
            {
                Id = x.Id,
                NombreCompleto = x.NombreCompleto,
                EmpresaRepresenta = x.EmpresaRepresenta,
                PaisOrigen = x.PaisOrigen,
                FechaInicioVisita = x.FechaInicioVisita,
                FechaFinVisita = x.FechaFinVisita,
                LugarHospedaje = x.LugarHospedaje,
                CodigosIdioma = x.Idiomas.Select(i => i.CodigoIdioma).ToList()
            })
            .ToListAsync();

        return Ok(inversores);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<InversorResponse>> GetById(int id)
    {
        var inversor = await _dbContext.Inversores
            .Include(x => x.Idiomas)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (inversor is null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(inversor));
    }

    [HttpPost]
    public async Task<ActionResult<InversorResponse>> Create(CrearInversorRequest request)
    {
        var codigosIdioma = request.CodigosIdioma
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim().ToLowerInvariant())
            .Distinct()
            .ToList();

        if (codigosIdioma.Count == 0)
        {
            return BadRequest("Debe indicar al menos un idioma para el inversor.");
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
            var inversor = new Inversor(
                request.NombreCompleto,
                request.EmpresaRepresenta,
                request.PaisOrigen,
                request.FechaInicioVisita,
                request.FechaFinVisita,
                request.LugarHospedaje,
                codigosIdioma);

            _dbContext.Inversores.Add(inversor);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = inversor.Id }, MapToResponse(inversor));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private static InversorResponse MapToResponse(Inversor inversor)
    {
        return new InversorResponse
        {
            Id = inversor.Id,
            NombreCompleto = inversor.NombreCompleto,
            EmpresaRepresenta = inversor.EmpresaRepresenta,
            PaisOrigen = inversor.PaisOrigen,
            FechaInicioVisita = inversor.FechaInicioVisita,
            FechaFinVisita = inversor.FechaFinVisita,
            LugarHospedaje = inversor.LugarHospedaje,
            CodigosIdioma = inversor.Idiomas.Select(i => i.CodigoIdioma).ToList()
        };
    }
}

using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/oficinas")]
public class OficinasController : ControllerBase
{
    private readonly CatalogDbContext _dbContext;

    public OficinasController(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<OficinaResponse>>> GetAll()
    {
        var oficinas = await _dbContext.Oficinas
            .OrderBy(x => x.Nombre)
            .Select(x => new OficinaResponse
            {
                Id = x.Id,
                Nombre = x.Nombre,
                Direccion = x.Direccion,
                Latitud = x.Latitud,
                Longitud = x.Longitud
            })
            .ToListAsync();

        return Ok(oficinas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OficinaResponse>> GetById(int id)
    {
        var oficina = await _dbContext.Oficinas.FirstOrDefaultAsync(x => x.Id == id);

        if (oficina is null)
        {
            return NotFound();
        }

        return Ok(new OficinaResponse
        {
            Id = oficina.Id,
            Nombre = oficina.Nombre,
            Direccion = oficina.Direccion,
            Latitud = oficina.Latitud,
            Longitud = oficina.Longitud
        });
    }

    [HttpPost]
    public async Task<ActionResult<OficinaResponse>> Create(CrearOficinaRequest request)
    {
        try
        {
            var oficina = new Oficina(request.Nombre, request.Direccion, request.Latitud, request.Longitud);

            _dbContext.Oficinas.Add(oficina);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = oficina.Id }, new OficinaResponse
            {
                Id = oficina.Id,
                Nombre = oficina.Nombre,
                Direccion = oficina.Direccion,
                Latitud = oficina.Latitud,
                Longitud = oficina.Longitud
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

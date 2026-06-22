using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/matriz-traslados")]
public class MatrizTrasladosController : ControllerBase
{
    private readonly CatalogDbContext _dbContext;

    public MatrizTrasladosController(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<MatrizTrasladoResponse>>> GetAll()
    {
        var traslados = await _dbContext.MatrizTraslados
            .OrderBy(x => x.IdOficinaOrigen)
            .ThenBy(x => x.IdOficinaDestino)
            .Select(x => new MatrizTrasladoResponse
            {
                Id = x.Id,
                IdOficinaOrigen = x.IdOficinaOrigen,
                IdOficinaDestino = x.IdOficinaDestino,
                TiempoMinutos = x.TiempoMinutos
            })
            .ToListAsync();

        return Ok(traslados);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CrearMatrizTrasladoRequest request)
    {
        if (request.IdOficinaOrigen == request.IdOficinaDestino)
        {
            return BadRequest("La oficina origen y destino deben ser diferentes.");
        }

        if (request.TiempoMinutos < 0)
        {
            return BadRequest("El tiempo de traslado no puede ser negativo.");
        }

        var oficinasExisten = await _dbContext.Oficinas
            .CountAsync(x => x.Id == request.IdOficinaOrigen || x.Id == request.IdOficinaDestino);

        if (oficinasExisten != 2)
        {
            return BadRequest("Una o ambas oficinas no existen.");
        }

        await UpsertTraslado(request.IdOficinaOrigen, request.IdOficinaDestino, request.TiempoMinutos);
        await UpsertTraslado(request.IdOficinaDestino, request.IdOficinaOrigen, request.TiempoMinutos);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    private async Task UpsertTraslado(int origen, int destino, int minutos)
    {
        var existente = await _dbContext.MatrizTraslados
            .FirstOrDefaultAsync(x => x.IdOficinaOrigen == origen && x.IdOficinaDestino == destino);

        if (existente is null)
        {
            _dbContext.MatrizTraslados.Add(new MatrizTraslado(origen, destino, minutos));
        }
        else
        {
            existente.UpdateTiempo(minutos);
        }
    }
}

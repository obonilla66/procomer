using Catalog.Application.DTOs;
using Catalog.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/idiomas")]
public class IdiomasController : ControllerBase
{
    private readonly CatalogDbContext _dbContext;

    public IdiomasController(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<IdiomaResponse>>> GetAll()
    {
        var idiomas = await _dbContext.Idiomas
            .OrderBy(x => x.NombreIdioma)
            .Select(x => new IdiomaResponse
            {
                CodigoIdioma = x.CodigoIdioma,
                Idioma = x.NombreIdioma
            })
            .ToListAsync();

        return Ok(idiomas);
    }
}

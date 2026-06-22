/*
 * Repositorio de matriz de traslado.
 *
 * Entrega al algoritmo todos los tiempos de traslado configurados
 * entre oficinas.
 */

using Agenda.Application.Interfaces;
using Agenda.Domain.Entities;
using Agenda.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Infrastructure.Repositories;

public class MatrizTrasladoRepository : IMatrizTrasladoRepository
{
    private readonly AgendaDbContext _context;

    public MatrizTrasladoRepository(AgendaDbContext context)
    {
        _context = context;
    }

    public Task<List<MatrizTraslado>> ObtenerTodosAsync(CancellationToken cancellationToken)
    {
        return _context.MatrizTraslados.ToListAsync(cancellationToken);
    }
}

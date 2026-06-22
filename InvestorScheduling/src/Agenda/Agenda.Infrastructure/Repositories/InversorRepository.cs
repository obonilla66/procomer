using Agenda.Application.Interfaces;
using Agenda.Domain.Entities;
using Agenda.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Infrastructure.Repositories;

public class InversorRepository : IInversorRepository
{
    private readonly AgendaDbContext _context;

    public InversorRepository(AgendaDbContext context)
    {
        _context = context;
    }

    public Task<Inversor?> ObtenerConIdiomasAsync(
        int idInversor,
        CancellationToken cancellationToken)
    {
        return _context.Inversores
            .Include(x => x.Idiomas)
            .FirstOrDefaultAsync(x => x.Id == idInversor, cancellationToken);
    }
}

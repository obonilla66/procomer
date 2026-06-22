using Agenda.Domain.Entities;

namespace Agenda.Application.Interfaces;

public interface IInversorRepository
{
    Task<Inversor?> ObtenerConIdiomasAsync(int idInversor, CancellationToken cancellationToken);
}

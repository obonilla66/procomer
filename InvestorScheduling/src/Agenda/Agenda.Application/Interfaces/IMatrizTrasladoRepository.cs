using Agenda.Domain.Entities;

namespace Agenda.Application.Interfaces;

public interface IMatrizTrasladoRepository
{
    Task<List<MatrizTraslado>> ObtenerTodosAsync(CancellationToken cancellationToken);
}

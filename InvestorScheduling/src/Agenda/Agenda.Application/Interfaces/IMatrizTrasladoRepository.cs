/*
 * Contrato para consultar la matriz de tiempos de traslado.
 *
 * El algoritmo usa esta información para validar que entre reuniones
 * consecutivas exista tiempo suficiente para mover al inversor.
 */

using Agenda.Domain.Entities;

namespace Agenda.Application.Interfaces;

public interface IMatrizTrasladoRepository
{
    Task<List<MatrizTraslado>> ObtenerTodosAsync(CancellationToken cancellationToken);
}

/*
 * Contrato para consultar información del inversor.
 *
 * Application solicita un inversor con sus idiomas.
 * Infrastructure decide cómo obtenerlo desde SQL Server.
 */

using Agenda.Domain.Entities;

namespace Agenda.Application.Interfaces;

public interface IInversorRepository
{
    Task<Inversor?> ObtenerConIdiomasAsync(int idInversor, CancellationToken cancellationToken);
}

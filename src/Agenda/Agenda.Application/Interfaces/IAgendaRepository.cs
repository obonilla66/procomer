
using Agenda.Application.Dtos;
using Agenda.Domain.Entities;

namespace Agenda.Application.Interfaces;

public interface IAgendaRepository
{
    Task<int> CrearAgendaAsync(
        AgendaEntity agenda,
        IReadOnlyCollection<int> participantesCandidatos,
        IReadOnlyCollection<AgendaDetalle> detalles,
        CancellationToken cancellationToken);

    Task<List<AgendaResumenResponse>> ListarAsync(
        int? idInversor,
        DateTime? fecha,
        bool? estado,
        CancellationToken cancellationToken);

    Task<AgendaDetalleCompletaResponse?> ObtenerDetalleAsync(
        int idAgenda,
        CancellationToken cancellationToken);

    Task<bool> AnularAsync(
        int idAgenda,
        CancellationToken cancellationToken);
}

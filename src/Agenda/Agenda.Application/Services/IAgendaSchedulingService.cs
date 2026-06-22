using Agenda.Application.Dtos;

namespace Agenda.Application.Services;

public interface IAgendaSchedulingService
{
    Task<AgendaGeneradaResponse> GenerarAsync(
        GenerarAgendaRequest request,
        CancellationToken cancellationToken);
}

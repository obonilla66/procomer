
using System.Net.Http.Json;
using Agenda.Application.Interfaces;
using Agenda.Domain.Entities;

namespace Agenda.Infrastructure.Clients;

public class CatalogInversorClientRepository : IInversorRepository
{
    private readonly HttpClient _httpClient;

    public CatalogInversorClientRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Inversor?> ObtenerConIdiomasAsync(
        int idInversor,
        CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetFromJsonAsync<InversorCatalogDto>(
            $"/api/inversores/{idInversor}",
            cancellationToken);

        if (response is null)
        {
            return null;
        }

        return new Inversor
        {
            Id = response.Id,
            NombreCompleto = response.NombreCompleto,
            EmpresaRepresenta = response.EmpresaRepresenta,
            PaisOrigen = response.PaisOrigen,
            FechaInicioVisita = response.FechaInicioVisita,
            FechaFinVisita = response.FechaFinVisita,
            LugarHospedaje = response.LugarHospedaje,
            Idiomas = response.CodigosIdioma
                .Select(codigo => new InversorIdioma
                {
                    IdInversor = response.Id,
                    CodigoIdioma = codigo
                })
                .ToList()
        };
    }
}

public class CatalogParticipanteClientRepository : IParticipanteRepository
{
    private readonly HttpClient _httpClient;

    public CatalogParticipanteClientRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Participante>> ObtenerCandidatosConDisponibilidadAsync(
        IReadOnlyCollection<int> idsParticipantes,
        DateTime fecha,
        CancellationToken cancellationToken)
    {
        var ids = string.Join(",", idsParticipantes);

        var participantes = await _httpClient.GetFromJsonAsync<List<ParticipanteCatalogDto>>(
            $"/api/participantes?ids={ids}",
            cancellationToken) ?? [];

        var horarios = await _httpClient.GetFromJsonAsync<List<ParticipanteHorarioCatalogDto>>(
            $"/api/participante-horarios?fecha={fecha:yyyy-MM-dd}&idsParticipantes={ids}",
            cancellationToken) ?? [];

        var horariosPorParticipante = horarios
            .GroupBy(x => x.IdParticipante)
            .ToDictionary(x => x.Key, x => x.ToList());

        return participantes
            .Where(x => x.Estado)
            .Where(x => horariosPorParticipante.ContainsKey(x.Id))
            .Select(participante => new Participante
            {
                Id = participante.Id,
                NombreCompleto = participante.NombreCompleto,
                Cargo = participante.Cargo,
                Institucion = participante.Institucion,
                IdOficina = participante.IdOficina,
                Estado = participante.Estado,
                Oficina = new Oficina
                {
                    Id = participante.IdOficina,
                    Nombre = participante.NombreOficina,
                    Direccion = participante.DireccionOficina
                },
                Idiomas = participante.CodigosIdioma
                    .Select(codigo => new ParticipanteIdioma
                    {
                        IdParticipante = participante.Id,
                        CodigoIdioma = codigo
                    })
                    .ToList(),
                Horarios = horariosPorParticipante[participante.Id]
                    .Select(horario => new ParticipanteHorario
                    {
                        Id = horario.Id,
                        IdParticipante = horario.IdParticipante,
                        FechaHoraInicio = horario.FechaHoraInicio,
                        FechaHoraFin = horario.FechaHoraFin
                    })
                    .ToList()
            })
            .ToList();
    }
}

public class CatalogMatrizTrasladoClientRepository : IMatrizTrasladoRepository
{
    private readonly HttpClient _httpClient;

    public CatalogMatrizTrasladoClientRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<MatrizTraslado>> ObtenerTodosAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetFromJsonAsync<List<MatrizTrasladoCatalogDto>>(
            "/api/matriz-traslados",
            cancellationToken) ?? [];

        return response
            .Select(x => new MatrizTraslado
            {
                Id = x.Id,
                IdOficinaOrigen = x.IdOficinaOrigen,
                IdOficinaDestino = x.IdOficinaDestino,
                TiempoMinutos = x.TiempoMinutos
            })
            .ToList();
    }
}

internal class InversorCatalogDto
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string? EmpresaRepresenta { get; set; }
    public string PaisOrigen { get; set; } = string.Empty;
    public DateTime FechaInicioVisita { get; set; }
    public DateTime FechaFinVisita { get; set; }
    public string LugarHospedaje { get; set; } = string.Empty;
    public List<string> CodigosIdioma { get; set; } = [];
}

internal class ParticipanteCatalogDto
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string? Cargo { get; set; }
    public string Institucion { get; set; } = string.Empty;
    public int IdOficina { get; set; }
    public string NombreOficina { get; set; } = string.Empty;
    public string DireccionOficina { get; set; } = string.Empty;
    public bool Estado { get; set; }
    public List<string> CodigosIdioma { get; set; } = [];
}

internal class ParticipanteHorarioCatalogDto
{
    public int Id { get; set; }
    public int IdParticipante { get; set; }
    public DateTime FechaHoraInicio { get; set; }
    public DateTime FechaHoraFin { get; set; }
}

internal class MatrizTrasladoCatalogDto
{
    public int Id { get; set; }
    public int IdOficinaOrigen { get; set; }
    public int IdOficinaDestino { get; set; }
    public int TiempoMinutos { get; set; }
}

using Agenda.Application.Dtos;
using Agenda.Application.Interfaces;
using Agenda.Domain.Entities;
using Agenda.Domain.Rules;

namespace Agenda.Application.Services;

public class AgendaSchedulingService : IAgendaSchedulingService
{
    private readonly IInversorRepository _inversorRepository;
    private readonly IParticipanteRepository _participanteRepository;
    private readonly IMatrizTrasladoRepository _matrizTrasladoRepository;
    private readonly IAgendaRepository _agendaRepository;

    public AgendaSchedulingService(
        IInversorRepository inversorRepository,
        IParticipanteRepository participanteRepository,
        IMatrizTrasladoRepository matrizTrasladoRepository,
        IAgendaRepository agendaRepository)
    {
        _inversorRepository = inversorRepository;
        _participanteRepository = participanteRepository;
        _matrizTrasladoRepository = matrizTrasladoRepository;
        _agendaRepository = agendaRepository;
    }

    public async Task<AgendaGeneradaResponse> GenerarAsync(
        GenerarAgendaRequest request,
        CancellationToken cancellationToken)
    {
        if (request.IdInversor <= 0)
            return Error("Debe indicar un inversor válido.");

        if (request.Fecha == default)
            return Error("Debe indicar una fecha válida.");

        if (request.DuracionReunionMinutos <= 0)
            return Error("La duración estándar de reunión debe ser mayor a cero.");

        if (request.CantidadReunionesMeta <= 0)
            return Error("La cantidad meta de reuniones debe ser mayor a cero.");

        if (request.IdsParticipantes is null || request.IdsParticipantes.Count == 0)
            return Error("Debe indicar al menos un participante candidato.");

        var fecha = request.Fecha.Date;

        var inversor = await _inversorRepository.ObtenerConIdiomasAsync(
            request.IdInversor,
            cancellationToken);

        if (inversor is null)
            return Error("El inversor indicado no existe.");

        if (!AgendaBusinessRules.FechaDentroVisita(
                fecha,
                inversor.FechaInicioVisita,
                inversor.FechaFinVisita))
        {
            return Error("La fecha indicada está fuera del período de visita del inversor.");
        }

        var idiomasInversor = inversor.Idiomas
            .Select(x => x.CodigoIdioma)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        if (idiomasInversor.Count == 0)
            return Error("El inversor no tiene idiomas configurados.");

        var participantes = await _participanteRepository.ObtenerCandidatosConDisponibilidadAsync(
            request.IdsParticipantes.Distinct().ToList(),
            fecha,
            cancellationToken);

        if (participantes.Count == 0)
            return Error("No hay participantes activos o con disponibilidad para la fecha solicitada.");

        var participantesConIdioma = participantes
            .Where(p => p.Idiomas.Any(i => idiomasInversor.Contains(i.CodigoIdioma)))
            .ToList();

        if (participantesConIdioma.Count == 0)
            return Error("Ningún participante candidato comparte idioma con el inversor.");

        var slots = GenerarSlots(
            participantesConIdioma,
            idiomasInversor,
            fecha,
            request.DuracionReunionMinutos);

        if (slots.Count == 0)
            return Error("No existe disponibilidad dentro del horario laboral excluyendo el almuerzo.");

        var traslados = await _matrizTrasladoRepository.ObtenerTodosAsync(cancellationToken);

        var mejorSecuencia = BuscarMejorSecuencia(
            slots,
            traslados,
            request.CantidadReunionesMeta);

        if (mejorSecuencia.Count == 0)
            return Error("No se encontró una combinación viable considerando horarios y traslados.");

        var agenda = new AgendaEntity
        {
            IdInversor = request.IdInversor,
            Fecha = fecha,
            DuracionReunionMinutos = request.DuracionReunionMinutos,
            CantidadReunionesMeta = request.CantidadReunionesMeta,
            Estado = true
        };

        var detalles = CrearDetalles(mejorSecuencia, traslados);

        var idAgenda = await _agendaRepository.CrearAgendaAsync(
            agenda,
            request.IdsParticipantes.Distinct().ToList(),
            detalles,
            cancellationToken);

        var totalTraslado = CalcularTrasladoTotal(mejorSecuencia, traslados);

        return new AgendaGeneradaResponse
        {
            Exitoso = true,
            Mensaje = mejorSecuencia.Count >= request.CantidadReunionesMeta
                ? "Agenda generada exitosamente."
                : $"No se alcanzó la meta completa. Se programaron {mejorSecuencia.Count} de {request.CantidadReunionesMeta} reuniones posibles.",
            IdAgenda = idAgenda,
            ReunionesProgramadas = mejorSecuencia.Count,
            TiempoTotalTrasladoMinutos = totalTraslado,
            Detalles = mejorSecuencia.Select((x, i) => new AgendaDetalleResponse
            {
                IdParticipante = x.IdParticipante,
                Participante = x.NombreParticipante,
                IdOficina = x.IdOficina,
                Oficina = x.NombreOficina,
                CodigoIdioma = x.CodigoIdioma,
                FechaHoraInicio = x.Inicio,
                FechaHoraFin = x.Fin,
                TiempoTrasladoSiguienteOficinaMinutos = i < mejorSecuencia.Count - 1
                    ? ObtenerTraslado(x.IdOficina, mejorSecuencia[i + 1].IdOficina, traslados)
                    : null
            }).ToList()
        };
    }

    private static List<AgendaDetalle> CrearDetalles(
        List<SlotReunion> slots,
        List<MatrizTraslado> traslados)
    {
        var detalles = new List<AgendaDetalle>();

        for (var i = 0; i < slots.Count; i++)
        {
            var actual = slots[i];

            detalles.Add(new AgendaDetalle
            {
                IdParticipante = actual.IdParticipante,
                CodigoIdioma = actual.CodigoIdioma,
                FechaHoraInicio = actual.Inicio,
                FechaHoraFin = actual.Fin,
                TiempoTrasladoSiguienteOficinaMinutos = i < slots.Count - 1
                    ? ObtenerTraslado(actual.IdOficina, slots[i + 1].IdOficina, traslados)
                    : null
            });
        }

        return detalles;
    }

    private static List<SlotReunion> GenerarSlots(
        List<Participante> participantes,
        HashSet<string> idiomasInversor,
        DateTime fecha,
        int duracionMinutos)
    {
        var resultado = new List<SlotReunion>();
        var duracion = TimeSpan.FromMinutes(duracionMinutos);

        var inicioDia = fecha.Add(AgendaBusinessRules.InicioLaboral);
        var finDia = fecha.Add(AgendaBusinessRules.FinLaboral);
        var inicioAlmuerzo = fecha.Add(AgendaBusinessRules.InicioAlmuerzo);
        var finAlmuerzo = fecha.Add(AgendaBusinessRules.FinAlmuerzo);

        foreach (var participante in participantes)
        {
            var codigoIdioma = participante.Idiomas
                .First(i => idiomasInversor.Contains(i.CodigoIdioma))
                .CodigoIdioma;

            foreach (var horario in participante.Horarios)
            {
                var inicioDisponible = Max(horario.FechaHoraInicio, inicioDia);
                var finDisponible = Min(horario.FechaHoraFin, finDia);

                AgregarSlots(
                    resultado,
                    participante,
                    codigoIdioma,
                    inicioDisponible,
                    Min(finDisponible, inicioAlmuerzo),
                    duracion);

                AgregarSlots(
                    resultado,
                    participante,
                    codigoIdioma,
                    Max(inicioDisponible, finAlmuerzo),
                    finDisponible,
                    duracion);
            }
        }

        return resultado
            .OrderBy(x => x.Inicio)
            .ThenBy(x => x.Fin)
            .ThenBy(x => x.IdParticipante)
            .ToList();
    }

    private static void AgregarSlots(
        List<SlotReunion> resultado,
        Participante participante,
        string codigoIdioma,
        DateTime inicio,
        DateTime fin,
        TimeSpan duracion)
    {
        if (inicio >= fin)
            return;

        for (var cursor = inicio; cursor + duracion <= fin; cursor += AgendaBusinessRules.PasoBusqueda)
        {
            var reunionInicio = cursor;
            var reunionFin = cursor + duracion;

            if (!AgendaBusinessRules.EstaDentroHorarioLaboral(reunionInicio, reunionFin))
                continue;

            if (AgendaBusinessRules.CruzaAlmuerzo(reunionInicio, reunionFin))
                continue;

            resultado.Add(new SlotReunion
            {
                IdParticipante = participante.Id,
                NombreParticipante = participante.NombreCompleto,
                IdOficina = participante.IdOficina,
                NombreOficina = participante.Oficina.Nombre,
                CodigoIdioma = codigoIdioma,
                Inicio = reunionInicio,
                Fin = reunionFin
            });
        }
    }

    private static List<SlotReunion> BuscarMejorSecuencia(
        List<SlotReunion> slots,
        List<MatrizTraslado> traslados,
        int meta)
    {
        var mejor = new List<SlotReunion>();

        void Backtrack(List<SlotReunion> actual, int indiceInicio)
        {
            if (EsMejor(actual, mejor, traslados))
                mejor = actual.ToList();

            if (actual.Count >= meta)
                return;

            var restantes = slots.Count - indiceInicio;
            if (actual.Count + restantes < mejor.Count)
                return;

            for (var i = indiceInicio; i < slots.Count; i++)
            {
                var candidato = slots[i];

                if (actual.Any(x => x.IdParticipante == candidato.IdParticipante))
                    continue;

                if (actual.Count > 0)
                {
                    var ultima = actual[^1];

                    var traslado = ObtenerTraslado(
                        ultima.IdOficina,
                        candidato.IdOficina,
                        traslados);

                    if (traslado is null)
                        continue;

                    var horaMinimaLlegada = ultima.Fin.AddMinutes(traslado.Value);

                    if (candidato.Inicio < horaMinimaLlegada)
                        continue;
                }

                actual.Add(candidato);
                Backtrack(actual, i + 1);
                actual.RemoveAt(actual.Count - 1);
            }
        }

        Backtrack([], 0);
        return mejor;
    }

    private static bool EsMejor(
        List<SlotReunion> actual,
        List<SlotReunion> mejor,
        List<MatrizTraslado> traslados)
    {
        if (actual.Count > mejor.Count)
            return true;

        if (actual.Count < mejor.Count)
            return false;

        if (actual.Count == 0)
            return false;

        var trasladoActual = CalcularTrasladoTotal(actual, traslados);
        var trasladoMejor = CalcularTrasladoTotal(mejor, traslados);

        if (trasladoActual < trasladoMejor)
            return true;

        if (trasladoActual > trasladoMejor)
            return false;

        return actual[^1].Fin < mejor[^1].Fin;
    }

    private static int CalcularTrasladoTotal(
        List<SlotReunion> agenda,
        List<MatrizTraslado> traslados)
    {
        var total = 0;

        for (var i = 0; i < agenda.Count - 1; i++)
        {
            var traslado = ObtenerTraslado(
                agenda[i].IdOficina,
                agenda[i + 1].IdOficina,
                traslados);

            if (traslado.HasValue)
                total += traslado.Value;
        }

        return total;
    }

    private static int? ObtenerTraslado(
        int idOficinaOrigen,
        int idOficinaDestino,
        List<MatrizTraslado> traslados)
    {
        if (idOficinaOrigen == idOficinaDestino)
            return 0;

        return traslados
            .FirstOrDefault(x =>
                x.IdOficinaOrigen == idOficinaOrigen &&
                x.IdOficinaDestino == idOficinaDestino)
            ?.TiempoMinutos;
    }

    private static DateTime Max(DateTime a, DateTime b) => a > b ? a : b;
    private static DateTime Min(DateTime a, DateTime b) => a < b ? a : b;

    private static AgendaGeneradaResponse Error(string mensaje)
    {
        return new AgendaGeneradaResponse
        {
            Exitoso = false,
            Mensaje = mensaje
        };
    }

    private sealed class SlotReunion
    {
        public int IdParticipante { get; set; }
        public string NombreParticipante { get; set; } = "";
        public int IdOficina { get; set; }
        public string NombreOficina { get; set; } = "";
        public string CodigoIdioma { get; set; } = "";
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
    }
}

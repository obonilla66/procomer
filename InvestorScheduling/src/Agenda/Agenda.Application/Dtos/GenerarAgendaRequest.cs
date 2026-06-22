/*
 * DTO de entrada para generar una agenda.
 *
 * Representa los parámetros que llegan desde la API:
 * - inversor;
 * - fecha específica;
 * - duración estándar;
 * - meta de reuniones;
 * - participantes candidatos.
 */

namespace Agenda.Application.Dtos;

public class GenerarAgendaRequest
{
    public int IdInversor { get; set; }
    public DateTime Fecha { get; set; }
    public int DuracionReunionMinutos { get; set; }
    public int CantidadReunionesMeta { get; set; }
    public List<int> IdsParticipantes { get; set; } = [];
}

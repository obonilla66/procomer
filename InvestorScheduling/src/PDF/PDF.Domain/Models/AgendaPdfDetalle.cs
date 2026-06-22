/*
 * Detalle de una reunión dentro de la agenda PDF.
 *
 * Incluye información enriquecida para el documento:
 * participante, cargo, oficina, dirección, idioma y traslado posterior.
 */

namespace PDF.Domain.Models;

public class AgendaPdfDetalle
{
    public DateTime FechaHoraInicio { get; set; }
    public DateTime FechaHoraFin { get; set; }
    public string NombreParticipante { get; set; } = "";
    public string? Cargo { get; set; }
    public string Institucion { get; set; } = "";
    public string Oficina { get; set; } = "";
    public string Direccion { get; set; } = "";
    public string CodigoIdioma { get; set; } = "";
    public string Idioma { get; set; } = "";
    public int? TiempoTrasladoSiguienteOficinaMinutos { get; set; }
}

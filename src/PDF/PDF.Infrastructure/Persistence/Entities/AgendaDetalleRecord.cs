/*
 * Entidad EF Core usada solo para mapear una tabla existente.
 *
 * Estas clases pertenecen a Infrastructure porque representan
 * el esquema físico de SQL Server, no el modelo de dominio del PDF.
 */

namespace PDF.Infrastructure.Persistence.Entities;

public class AgendaDetalleRecord
{
    public int Id { get; set; }
    public int IdAgenda { get; set; }
    public DateTime FechaHoraInicio { get; set; }
    public DateTime FechaHoraFin { get; set; }
    public int IdParticipante { get; set; }
    public string CodigoIdioma { get; set; } = "";
    public int? TiempoTrasladoSiguienteOficinaMinutos { get; set; }

    public AgendaRecord Agenda { get; set; } = null!;
    public ParticipanteRecord Participante { get; set; } = null!;
    public IdiomaRecord Idioma { get; set; } = null!;
}

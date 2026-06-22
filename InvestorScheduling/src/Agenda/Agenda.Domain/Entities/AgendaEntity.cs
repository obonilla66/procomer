/*
 * Entidad de dominio del microservicio Agenda.
 *
 * Estas clases representan el modelo principal del negocio.
 * No contienen referencias a EF Core, SQL Server ni detalles técnicos.
 */

namespace Agenda.Domain.Entities;

public class AgendaEntity
{
    public int Id { get; set; }
    public int IdInversor { get; set; }
    public DateTime Fecha { get; set; }
    public int DuracionReunionMinutos { get; set; }
    public int CantidadReunionesMeta { get; set; }
    public bool Estado { get; set; } = true;

    public Inversor Inversor { get; set; } = null!;
    public ICollection<AgendaParticipante> Participantes { get; set; } = [];
    public ICollection<AgendaDetalle> Detalles { get; set; } = [];
}

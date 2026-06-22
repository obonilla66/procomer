/*
 * Entidad de dominio del microservicio Agenda.
 *
 * Estas clases representan el modelo principal del negocio.
 * No contienen referencias a EF Core, SQL Server ni detalles técnicos.
 */

namespace Agenda.Domain.Entities;

public class AgendaParticipante
{
    public int Id { get; set; }
    public int IdAgenda { get; set; }
    public int IdParticipante { get; set; }

    public AgendaEntity Agenda { get; set; } = null!;
    public Participante Participante { get; set; } = null!;
}

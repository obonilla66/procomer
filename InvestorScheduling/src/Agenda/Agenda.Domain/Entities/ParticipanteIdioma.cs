/*
 * Entidad de dominio del microservicio Agenda.
 *
 * Estas clases representan el modelo principal del negocio.
 * No contienen referencias a EF Core, SQL Server ni detalles técnicos.
 */

namespace Agenda.Domain.Entities;

public class ParticipanteIdioma
{
    public int Id { get; set; }
    public int IdParticipante { get; set; }
    public string CodigoIdioma { get; set; } = null!;

    public Participante Participante { get; set; } = null!;
    public Idioma Idioma { get; set; } = null!;
}

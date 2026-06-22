/*
 * Entidad de dominio del microservicio Agenda.
 *
 * Estas clases representan el modelo principal del negocio.
 * No contienen referencias a EF Core, SQL Server ni detalles técnicos.
 */

namespace Agenda.Domain.Entities;

public class Idioma
{
    public string CodigoIdioma { get; set; } = null!;
    public string IdiomaNombre { get; set; } = null!;

    public ICollection<InversorIdioma> InversorIdiomas { get; set; } = [];
    public ICollection<ParticipanteIdioma> ParticipanteIdiomas { get; set; } = [];
}

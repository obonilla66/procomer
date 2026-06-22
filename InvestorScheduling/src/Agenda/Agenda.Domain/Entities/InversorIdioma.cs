/*
 * Entidad de dominio del microservicio Agenda.
 *
 * Estas clases representan el modelo principal del negocio.
 * No contienen referencias a EF Core, SQL Server ni detalles técnicos.
 */

namespace Agenda.Domain.Entities;

public class InversorIdioma
{
    public int Id { get; set; }
    public int IdInversor { get; set; }
    public string CodigoIdioma { get; set; } = null!;

    public Inversor Inversor { get; set; } = null!;
    public Idioma Idioma { get; set; } = null!;
}

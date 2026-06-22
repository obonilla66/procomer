/*
 * Entidad EF Core usada solo para mapear una tabla existente.
 *
 * Estas clases pertenecen a Infrastructure porque representan
 * el esquema físico de SQL Server, no el modelo de dominio del PDF.
 */

namespace PDF.Infrastructure.Persistence.Entities;

public class IdiomaRecord
{
    public string CodigoIdioma { get; set; } = "";
    public string Idioma { get; set; } = "";
}

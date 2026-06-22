/*
 * Entidad EF Core usada solo para mapear una tabla existente.
 *
 * Estas clases pertenecen a Infrastructure porque representan
 * el esquema físico de SQL Server, no el modelo de dominio del PDF.
 */

namespace PDF.Infrastructure.Persistence.Entities;

public class OficinaRecord
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Direccion { get; set; } = "";
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
}

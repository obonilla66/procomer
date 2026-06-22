/*
 * Entidad EF Core usada solo para mapear una tabla existente.
 *
 * Estas clases pertenecen a Infrastructure porque representan
 * el esquema físico de SQL Server, no el modelo de dominio del PDF.
 */

namespace PDF.Infrastructure.Persistence.Entities;

public class InversorRecord
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = "";
    public string? EmpresaRepresenta { get; set; }
    public string PaisOrigen { get; set; } = "";
    public DateTime FechaInicioVisita { get; set; }
    public DateTime FechaFinVisita { get; set; }
    public string LugarHospedaje { get; set; } = "";
}

/*
 * Entidad EF Core usada solo para mapear una tabla existente.
 *
 * Estas clases pertenecen a Infrastructure porque representan
 * el esquema físico de SQL Server, no el modelo de dominio del PDF.
 */

namespace PDF.Infrastructure.Persistence.Entities;

public class ParticipanteRecord
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = "";
    public string? Cargo { get; set; }
    public string Institucion { get; set; } = "";
    public int IdOficina { get; set; }
    public bool Estado { get; set; }

    public OficinaRecord Oficina { get; set; } = null!;
}

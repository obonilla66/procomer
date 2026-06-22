/*
 * Entidad EF Core usada solo para mapear una tabla existente.
 *
 * Estas clases pertenecen a Infrastructure porque representan
 * el esquema físico de SQL Server, no el modelo de dominio del PDF.
 */

namespace PDF.Infrastructure.Persistence.Entities;

public class AgendaRecord
{
    public int Id { get; set; }
    public int IdInversor { get; set; }
    public DateTime Fecha { get; set; }
    public int DuracionReunionMinutos { get; set; }
    public int CantidadReunionesMeta { get; set; }
    public bool Estado { get; set; }

    public InversorRecord Inversor { get; set; } = null!;
    public ICollection<AgendaDetalleRecord> Detalles { get; set; } = [];
}

/*
 * Modelo de dominio específico para la generación del PDF.
 *
 * No representa necesariamente una tabla de base de datos.
 * Representa la información consolidada que el documento necesita.
 */

namespace PDF.Domain.Models;

public class AgendaPdfData
{
    public int IdAgenda { get; set; }
    public DateTime Fecha { get; set; }
    public string NombreInversor { get; set; } = "";
    public string? EmpresaRepresenta { get; set; }
    public string PaisOrigen { get; set; } = "";
    public bool Estado { get; set; }
    public List<AgendaPdfDetalle> Detalles { get; set; } = [];
}

namespace Agenda.Domain.Entities;

public class MatrizTraslado
{
    public int Id { get; set; }
    public int IdOficinaOrigen { get; set; }
    public int IdOficinaDestino { get; set; }
    public int TiempoMinutos { get; set; }

    public Oficina OficinaOrigen { get; set; } = null!;
    public Oficina OficinaDestino { get; set; } = null!;
}

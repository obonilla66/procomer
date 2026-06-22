namespace Agenda.Domain.Entities;

public class Oficina
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Direccion { get; set; } = null!;
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }

    public ICollection<Participante> Participantes { get; set; } = [];
}

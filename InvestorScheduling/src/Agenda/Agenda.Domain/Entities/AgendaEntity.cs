namespace Agenda.Domain.Entities;

public class AgendaEntity
{
    public int Id { get; set; }
    public int IdInversor { get; set; }
    public DateTime Fecha { get; set; }
    public int DuracionReunionMinutos { get; set; }
    public int CantidadReunionesMeta { get; set; }
    public bool Estado { get; set; } = true;

    public Inversor Inversor { get; set; } = null!;
    public ICollection<AgendaParticipante> Participantes { get; set; } = [];
    public ICollection<AgendaDetalle> Detalles { get; set; } = [];
}

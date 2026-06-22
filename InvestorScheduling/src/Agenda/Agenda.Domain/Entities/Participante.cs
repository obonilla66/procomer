namespace Agenda.Domain.Entities;

public class Participante
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = null!;
    public string? Cargo { get; set; }
    public string Institucion { get; set; } = null!;
    public int IdOficina { get; set; }
    public bool Estado { get; set; } = true;

    public Oficina Oficina { get; set; } = null!;
    public ICollection<ParticipanteIdioma> Idiomas { get; set; } = [];
    public ICollection<ParticipanteHorario> Horarios { get; set; } = [];
    public ICollection<AgendaParticipante> AgendaParticipantes { get; set; } = [];
    public ICollection<AgendaDetalle> AgendaDetalles { get; set; } = [];
}

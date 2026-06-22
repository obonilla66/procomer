namespace Agenda.Domain.Entities;

public class ParticipanteHorario
{
    public int Id { get; set; }
    public int IdParticipante { get; set; }
    public DateTime FechaHoraInicio { get; set; }
    public DateTime FechaHoraFin { get; set; }

    public Participante Participante { get; set; } = null!;
}

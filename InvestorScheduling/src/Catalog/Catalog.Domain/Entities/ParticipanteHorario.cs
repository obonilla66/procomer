namespace Catalog.Domain.Entities;

public class ParticipanteHorario
{
    public int Id { get; private set; }
    public int IdParticipante { get; private set; }
    public DateTime FechaHoraInicio { get; private set; }
    public DateTime FechaHoraFin { get; private set; }

    public Participante? Participante { get; private set; }

    private ParticipanteHorario()
    {
    }

    public ParticipanteHorario(int idParticipante, DateTime fechaHoraInicio, DateTime fechaHoraFin)
    {
        if (idParticipante <= 0)
        {
            throw new InvalidOperationException("El participante es requerido.");
        }

        if (fechaHoraFin <= fechaHoraInicio)
        {
            throw new InvalidOperationException("La fecha/hora final debe ser mayor que la fecha/hora inicial.");
        }

        IdParticipante = idParticipante;
        FechaHoraInicio = fechaHoraInicio;
        FechaHoraFin = fechaHoraFin;
    }
}

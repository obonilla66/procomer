namespace Agenda.Application.Dtos;

public class AgendaGeneradaResponse
{
    public bool Exitoso { get; set; }
    public string Mensaje { get; set; } = "";
    public int? IdAgenda { get; set; }
    public int ReunionesProgramadas { get; set; }
    public int TiempoTotalTrasladoMinutos { get; set; }
    public List<AgendaDetalleResponse> Detalles { get; set; } = [];
}

public class AgendaDetalleResponse
{
    public int IdParticipante { get; set; }
    public string Participante { get; set; } = "";
    public int IdOficina { get; set; }
    public string Oficina { get; set; } = "";
    public string CodigoIdioma { get; set; } = "";
    public DateTime FechaHoraInicio { get; set; }
    public DateTime FechaHoraFin { get; set; }
    public int? TiempoTrasladoSiguienteOficinaMinutos { get; set; }
}

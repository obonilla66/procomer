
namespace InvestorScheduling.Web.Models;

public class GenerarAgendaViewModel
{
    public int IdInversor { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Today;
    public int DuracionReunionMinutos { get; set; } = 60;
    public int CantidadReunionesMeta { get; set; } = 3;
    public List<int> IdsParticipantes { get; set; } = [];
    public List<InversorResponse> Inversores { get; set; } = [];
    public List<ParticipanteResponse> Participantes { get; set; } = [];
}

public class GenerarAgendaRequest
{
    public int IdInversor { get; set; }
    public DateTime Fecha { get; set; }
    public int DuracionReunionMinutos { get; set; }
    public int CantidadReunionesMeta { get; set; }
    public List<int> IdsParticipantes { get; set; } = [];
}

public class AgendaGeneradaResponse
{
    public bool Exitoso { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public int? IdAgenda { get; set; }
    public int ReunionesProgramadas { get; set; }
    public int TiempoTotalTrasladoMinutos { get; set; }
    public List<AgendaDetalleResponse> Detalles { get; set; } = [];
}

public class AgendaDetalleResponse
{
    public int IdParticipante { get; set; }
    public string Participante { get; set; } = string.Empty;
    public int IdOficina { get; set; }
    public string Oficina { get; set; } = string.Empty;
    public string CodigoIdioma { get; set; } = string.Empty;
    public DateTime FechaHoraInicio { get; set; }
    public DateTime FechaHoraFin { get; set; }
    public int? TiempoTrasladoSiguienteOficinaMinutos { get; set; }
}

public class AgendaResumenResponse
{
    public int Id { get; set; }
    public int IdInversor { get; set; }
    public DateTime Fecha { get; set; }
    public int DuracionReunionMinutos { get; set; }
    public int CantidadReunionesMeta { get; set; }
    public bool Estado { get; set; }
    public int CantidadReunionesProgramadas { get; set; }
}

public class AgendaDetalleCompletaResponse
{
    public int Id { get; set; }
    public int IdInversor { get; set; }
    public DateTime Fecha { get; set; }
    public int DuracionReunionMinutos { get; set; }
    public int CantidadReunionesMeta { get; set; }
    public bool Estado { get; set; }
    public List<AgendaDetalleResponse> Detalles { get; set; } = [];
}

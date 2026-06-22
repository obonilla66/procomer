
namespace Agenda.Application.Dtos;

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

public class PdfFileResponse
{
    public bool Exitoso { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public string FileName { get; set; } = "agenda.pdf";
    public string ContentType { get; set; } = "application/pdf";
    public byte[] Content { get; set; } = [];
}

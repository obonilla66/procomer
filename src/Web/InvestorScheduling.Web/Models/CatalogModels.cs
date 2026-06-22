
namespace InvestorScheduling.Web.Models;

public class InversorResponse
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string? EmpresaRepresenta { get; set; }
    public string PaisOrigen { get; set; } = string.Empty;
    public DateTime FechaInicioVisita { get; set; }
    public DateTime FechaFinVisita { get; set; }
    public string LugarHospedaje { get; set; } = string.Empty;
    public List<string> CodigosIdioma { get; set; } = [];
}

public class CrearInversorRequest
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string? EmpresaRepresenta { get; set; }
    public string PaisOrigen { get; set; } = string.Empty;
    public DateTime FechaInicioVisita { get; set; } = DateTime.Today;
    public DateTime FechaFinVisita { get; set; } = DateTime.Today;
    public string LugarHospedaje { get; set; } = string.Empty;
    public List<string> CodigosIdioma { get; set; } = [];
}

public class ParticipanteResponse
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string? Cargo { get; set; }
    public string Institucion { get; set; } = string.Empty;
    public int IdOficina { get; set; }
    public string NombreOficina { get; set; } = string.Empty;
    public string DireccionOficina { get; set; } = string.Empty;
    public bool Estado { get; set; }
    public List<string> CodigosIdioma { get; set; } = [];
}

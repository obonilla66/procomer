namespace Catalog.Application.DTOs;

public class IdiomaResponse
{
    public string CodigoIdioma { get; set; } = string.Empty;
    public string Idioma { get; set; } = string.Empty;
}

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
    public DateTime FechaInicioVisita { get; set; }
    public DateTime FechaFinVisita { get; set; }
    public string LugarHospedaje { get; set; } = string.Empty;
    public List<string> CodigosIdioma { get; set; } = [];
}

public class OficinaResponse
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
}

public class CrearOficinaRequest
{
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
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

public class CrearParticipanteRequest
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string? Cargo { get; set; }
    public string Institucion { get; set; } = string.Empty;
    public int IdOficina { get; set; }
    public bool Estado { get; set; } = true;
    public List<string> CodigosIdioma { get; set; } = [];
}

public class ParticipanteHorarioResponse
{
    public int Id { get; set; }
    public int IdParticipante { get; set; }
    public DateTime FechaHoraInicio { get; set; }
    public DateTime FechaHoraFin { get; set; }
}

public class CrearParticipanteHorarioRequest
{
    public int IdParticipante { get; set; }
    public DateTime FechaHoraInicio { get; set; }
    public DateTime FechaHoraFin { get; set; }
}

public class MatrizTrasladoResponse
{
    public int Id { get; set; }
    public int IdOficinaOrigen { get; set; }
    public int IdOficinaDestino { get; set; }
    public int TiempoMinutos { get; set; }
}

public class CrearMatrizTrasladoRequest
{
    public int IdOficinaOrigen { get; set; }
    public int IdOficinaDestino { get; set; }
    public int TiempoMinutos { get; set; }
}

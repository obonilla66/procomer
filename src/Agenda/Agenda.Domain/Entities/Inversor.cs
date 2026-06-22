namespace Agenda.Domain.Entities;

public class Inversor
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = null!;
    public string? EmpresaRepresenta { get; set; }
    public string PaisOrigen { get; set; } = null!;
    public DateTime FechaInicioVisita { get; set; }
    public DateTime FechaFinVisita { get; set; }
    public string LugarHospedaje { get; set; } = null!;

    public ICollection<InversorIdioma> Idiomas { get; set; } = [];
    public ICollection<AgendaEntity> Agendas { get; set; } = [];
}

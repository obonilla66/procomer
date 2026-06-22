namespace Catalog.Domain.Entities;

public class Inversor
{
    public int Id { get; private set; }
    public string NombreCompleto { get; private set; } = string.Empty;
    public string? EmpresaRepresenta { get; private set; }
    public string PaisOrigen { get; private set; } = string.Empty;
    public DateTime FechaInicioVisita { get; private set; }
    public DateTime FechaFinVisita { get; private set; }
    public string LugarHospedaje { get; private set; } = string.Empty;

    public List<InversorIdioma> Idiomas { get; private set; } = [];

    private Inversor()
    {
    }

    public Inversor(
        string nombreCompleto,
        string? empresaRepresenta,
        string paisOrigen,
        DateTime fechaInicioVisita,
        DateTime fechaFinVisita,
        string lugarHospedaje,
        IEnumerable<string> codigosIdioma)
    {
        Validate(nombreCompleto, paisOrigen, fechaInicioVisita, fechaFinVisita, lugarHospedaje, codigosIdioma);

        NombreCompleto = nombreCompleto.Trim();
        EmpresaRepresenta = string.IsNullOrWhiteSpace(empresaRepresenta) ? null : empresaRepresenta.Trim();
        PaisOrigen = paisOrigen.Trim();
        FechaInicioVisita = fechaInicioVisita.Date;
        FechaFinVisita = fechaFinVisita.Date;
        LugarHospedaje = lugarHospedaje.Trim();

        Idiomas = NormalizeLanguages(codigosIdioma)
            .Select(codigo => new InversorIdioma(Id, codigo))
            .ToList();
    }

    public void Update(
        string nombreCompleto,
        string? empresaRepresenta,
        string paisOrigen,
        DateTime fechaInicioVisita,
        DateTime fechaFinVisita,
        string lugarHospedaje,
        IEnumerable<string> codigosIdioma)
    {
        Validate(nombreCompleto, paisOrigen, fechaInicioVisita, fechaFinVisita, lugarHospedaje, codigosIdioma);

        NombreCompleto = nombreCompleto.Trim();
        EmpresaRepresenta = string.IsNullOrWhiteSpace(empresaRepresenta) ? null : empresaRepresenta.Trim();
        PaisOrigen = paisOrigen.Trim();
        FechaInicioVisita = fechaInicioVisita.Date;
        FechaFinVisita = fechaFinVisita.Date;
        LugarHospedaje = lugarHospedaje.Trim();

        Idiomas.Clear();

        foreach (var codigo in NormalizeLanguages(codigosIdioma))
        {
            Idiomas.Add(new InversorIdioma(Id, codigo));
        }
    }

    private static void Validate(
        string nombreCompleto,
        string paisOrigen,
        DateTime fechaInicioVisita,
        DateTime fechaFinVisita,
        string lugarHospedaje,
        IEnumerable<string> codigosIdioma)
    {
        if (string.IsNullOrWhiteSpace(nombreCompleto))
        {
            throw new InvalidOperationException("El nombre completo del inversor es requerido.");
        }

        if (string.IsNullOrWhiteSpace(paisOrigen))
        {
            throw new InvalidOperationException("El país de origen del inversor es requerido.");
        }

        if (string.IsNullOrWhiteSpace(lugarHospedaje))
        {
            throw new InvalidOperationException("El lugar de hospedaje del inversor es requerido.");
        }

        if (fechaFinVisita.Date < fechaInicioVisita.Date)
        {
            throw new InvalidOperationException("La fecha fin de visita no puede ser anterior a la fecha de inicio.");
        }

        if (!NormalizeLanguages(codigosIdioma).Any())
        {
            throw new InvalidOperationException("El inversor debe tener al menos un idioma.");
        }
    }

    private static List<string> NormalizeLanguages(IEnumerable<string> codigosIdioma)
    {
        return codigosIdioma
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim().ToLowerInvariant())
            .Distinct()
            .ToList();
    }
}

namespace Catalog.Domain.Entities;

public class Participante
{
    public int Id { get; private set; }
    public string NombreCompleto { get; private set; } = string.Empty;
    public string? Cargo { get; private set; }
    public string Institucion { get; private set; } = string.Empty;
    public int IdOficina { get; private set; }
    public bool Estado { get; private set; } = true;

    public Oficina? Oficina { get; private set; }
    public List<ParticipanteIdioma> Idiomas { get; private set; } = [];
    public List<ParticipanteHorario> Horarios { get; private set; } = [];

    private Participante()
    {
    }

    public Participante(
        string nombreCompleto,
        string? cargo,
        string institucion,
        int idOficina,
        IEnumerable<string> codigosIdioma,
        bool estado = true)
    {
        if (string.IsNullOrWhiteSpace(nombreCompleto))
        {
            throw new InvalidOperationException("El nombre completo del participante es requerido.");
        }

        if (string.IsNullOrWhiteSpace(institucion))
        {
            throw new InvalidOperationException("La institución del participante es requerida.");
        }

        if (idOficina <= 0)
        {
            throw new InvalidOperationException("El participante debe tener una oficina asignada.");
        }

        var idiomas = NormalizeLanguages(codigosIdioma);

        if (!idiomas.Any())
        {
            throw new InvalidOperationException("El participante debe tener al menos un idioma.");
        }

        NombreCompleto = nombreCompleto.Trim();
        Cargo = string.IsNullOrWhiteSpace(cargo) ? null : cargo.Trim();
        Institucion = institucion.Trim();
        IdOficina = idOficina;
        Estado = estado;

        Idiomas = idiomas
            .Select(codigo => new ParticipanteIdioma(Id, codigo))
            .ToList();
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

namespace Catalog.Domain.Entities;

public class Idioma
{
    public string CodigoIdioma { get; private set; } = string.Empty;
    public string NombreIdioma { get; private set; } = string.Empty;

    private Idioma()
    {
    }

    public Idioma(string codigoIdioma, string nombreIdioma)
    {
        if (string.IsNullOrWhiteSpace(codigoIdioma))
        {
            throw new InvalidOperationException("El código del idioma es requerido.");
        }

        if (string.IsNullOrWhiteSpace(nombreIdioma))
        {
            throw new InvalidOperationException("El nombre del idioma es requerido.");
        }

        CodigoIdioma = codigoIdioma.Trim().ToLowerInvariant();
        NombreIdioma = nombreIdioma.Trim();
    }
}

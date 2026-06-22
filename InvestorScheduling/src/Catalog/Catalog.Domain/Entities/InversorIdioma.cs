namespace Catalog.Domain.Entities;

public class InversorIdioma
{
    public int Id { get; private set; }
    public int IdInversor { get; private set; }
    public string CodigoIdioma { get; private set; } = string.Empty;

    public Inversor? Inversor { get; private set; }
    public Idioma? Idioma { get; private set; }

    private InversorIdioma()
    {
    }

    public InversorIdioma(int idInversor, string codigoIdioma)
    {
        if (string.IsNullOrWhiteSpace(codigoIdioma))
        {
            throw new InvalidOperationException("El código de idioma es requerido.");
        }

        IdInversor = idInversor;
        CodigoIdioma = codigoIdioma.Trim().ToLowerInvariant();
    }
}

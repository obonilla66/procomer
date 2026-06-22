namespace Catalog.Domain.Entities;

public class ParticipanteIdioma
{
    public int Id { get; private set; }
    public int IdParticipante { get; private set; }
    public string CodigoIdioma { get; private set; } = string.Empty;

    public Participante? Participante { get; private set; }
    public Idioma? Idioma { get; private set; }

    private ParticipanteIdioma()
    {
    }

    public ParticipanteIdioma(int idParticipante, string codigoIdioma)
    {
        if (string.IsNullOrWhiteSpace(codigoIdioma))
        {
            throw new InvalidOperationException("El código de idioma es requerido.");
        }

        IdParticipante = idParticipante;
        CodigoIdioma = codigoIdioma.Trim().ToLowerInvariant();
    }
}

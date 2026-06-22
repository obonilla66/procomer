namespace Agenda.Domain.Entities;

public class Idioma
{
    public string CodigoIdioma { get; set; } = null!;
    public string IdiomaNombre { get; set; } = null!;

    public ICollection<InversorIdioma> InversorIdiomas { get; set; } = [];
    public ICollection<ParticipanteIdioma> ParticipanteIdiomas { get; set; } = [];
}

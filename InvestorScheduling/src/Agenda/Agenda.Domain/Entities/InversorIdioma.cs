namespace Agenda.Domain.Entities;

public class InversorIdioma
{
    public int Id { get; set; }
    public int IdInversor { get; set; }
    public string CodigoIdioma { get; set; } = null!;

    public Inversor Inversor { get; set; } = null!;
    public Idioma Idioma { get; set; } = null!;
}

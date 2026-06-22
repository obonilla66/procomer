namespace Catalog.Domain.Entities;

public class Oficina
{
    public int Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Direccion { get; private set; } = string.Empty;
    public decimal? Latitud { get; private set; }
    public decimal? Longitud { get; private set; }

    public List<Participante> Participantes { get; private set; } = [];

    private Oficina()
    {
    }

    public Oficina(string nombre, string direccion, decimal? latitud = null, decimal? longitud = null)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new InvalidOperationException("El nombre de la oficina es requerido.");
        }

        if (string.IsNullOrWhiteSpace(direccion))
        {
            throw new InvalidOperationException("La dirección de la oficina es requerida.");
        }

        Nombre = nombre.Trim();
        Direccion = direccion.Trim();
        Latitud = latitud;
        Longitud = longitud;
    }

    public void Update(string nombre, string direccion, decimal? latitud = null, decimal? longitud = null)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new InvalidOperationException("El nombre de la oficina es requerido.");
        }

        if (string.IsNullOrWhiteSpace(direccion))
        {
            throw new InvalidOperationException("La dirección de la oficina es requerida.");
        }

        Nombre = nombre.Trim();
        Direccion = direccion.Trim();
        Latitud = latitud;
        Longitud = longitud;
    }
}

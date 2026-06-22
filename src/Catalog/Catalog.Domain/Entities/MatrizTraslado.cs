namespace Catalog.Domain.Entities;

public class MatrizTraslado
{
    public int Id { get; private set; }
    public int IdOficinaOrigen { get; private set; }
    public int IdOficinaDestino { get; private set; }
    public int TiempoMinutos { get; private set; }

    public Oficina? OficinaOrigen { get; private set; }
    public Oficina? OficinaDestino { get; private set; }

    private MatrizTraslado()
    {
    }

    public MatrizTraslado(int idOficinaOrigen, int idOficinaDestino, int tiempoMinutos)
    {
        if (idOficinaOrigen <= 0)
        {
            throw new InvalidOperationException("La oficina origen es requerida.");
        }

        if (idOficinaDestino <= 0)
        {
            throw new InvalidOperationException("La oficina destino es requerida.");
        }

        if (idOficinaOrigen == idOficinaDestino)
        {
            throw new InvalidOperationException("La oficina origen y destino deben ser diferentes.");
        }

        if (tiempoMinutos < 0)
        {
            throw new InvalidOperationException("El tiempo de traslado no puede ser negativo.");
        }

        IdOficinaOrigen = idOficinaOrigen;
        IdOficinaDestino = idOficinaDestino;
        TiempoMinutos = tiempoMinutos;
    }

    public void UpdateTiempo(int tiempoMinutos)
    {
        if (tiempoMinutos < 0)
        {
            throw new InvalidOperationException("El tiempo de traslado no puede ser negativo.");
        }

        TiempoMinutos = tiempoMinutos;
    }
}

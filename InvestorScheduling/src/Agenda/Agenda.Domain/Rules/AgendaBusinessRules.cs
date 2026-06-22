/*
 * Reglas de negocio puras del dominio de Agenda.
 *
 * Aquí se centralizan reglas que no dependen de infraestructura:
 * - horario laboral;
 * - bloque de almuerzo;
 * - validación de ventana de visita;
 * - verificación de reuniones dentro de horario.
 */

namespace Agenda.Domain.Rules;

public static class AgendaBusinessRules
{
    public static readonly TimeSpan InicioLaboral = new(8, 0, 0);
    public static readonly TimeSpan FinLaboral = new(17, 0, 0);
    public static readonly TimeSpan InicioAlmuerzo = new(12, 0, 0);
    public static readonly TimeSpan FinAlmuerzo = new(13, 0, 0);
    public static readonly TimeSpan PasoBusqueda = TimeSpan.FromMinutes(15);

    public static bool EstaDentroHorarioLaboral(DateTime inicio, DateTime fin)
    {
        return inicio.TimeOfDay >= InicioLaboral && fin.TimeOfDay <= FinLaboral;
    }

    public static bool CruzaAlmuerzo(DateTime inicio, DateTime fin)
    {
        var inicioAlmuerzo = inicio.Date.Add(InicioAlmuerzo);
        var finAlmuerzo = inicio.Date.Add(FinAlmuerzo);
        return inicio < finAlmuerzo && fin > inicioAlmuerzo;
    }

    public static bool FechaDentroVisita(DateTime fecha, DateTime inicioVisita, DateTime finVisita)
    {
        var dia = fecha.Date;
        return dia >= inicioVisita.Date && dia <= finVisita.Date;
    }
}

/*
 * Reglas de negocio del microservicio PDF.
 *
 * Se mantienen en Domain para que no dependan de infraestructura.
 */

using PDF.Domain.Models;

namespace PDF.Domain.Rules;

public static class PdfBusinessRules
{
    public static bool EsAgendaConfirmada(AgendaPdfData agenda)
    {
        return agenda.Estado && agenda.Detalles.Count > 0;
    }
}

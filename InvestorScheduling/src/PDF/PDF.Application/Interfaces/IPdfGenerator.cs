/*
 * Contrato para generar el archivo PDF.
 *
 * Permite cambiar la librería de generación de PDF en el futuro
 * sin modificar el caso de uso.
 */

using PDF.Domain.Models;

namespace PDF.Application.Interfaces;

public interface IPdfGenerator
{
    byte[] GenerarAgendaPdf(AgendaPdfData agenda);
}

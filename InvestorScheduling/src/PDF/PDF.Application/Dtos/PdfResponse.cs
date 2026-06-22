/*
 * DTO de respuesta del caso de uso.
 *
 * Contiene:
 * - Indicador de éxito.
 * - Mensaje funcional.
 * - Nombre sugerido del archivo.
 * - Content-Type.
 * - Bytes del PDF generado.
 */

namespace PDF.Application.Dtos;

public class PdfResponse
{
    public bool Exitoso { get; set; }
    public string Mensaje { get; set; } = "";
    public string FileName { get; set; } = "";
    public string ContentType { get; set; } = "application/pdf";
    public byte[] Content { get; set; } = [];
}

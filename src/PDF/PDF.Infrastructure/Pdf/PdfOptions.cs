/*
 * Opciones configurables del PDF.
 *
 * Se leen desde appsettings.json:
 *
 * "Pdf": {
 *   "InstitutionName": "PROCOMER",
 *   "LogoPath": "Assets/logo.png"
 * }
 */

namespace PDF.Infrastructure.Pdf;

public class PdfOptions
{
    public string InstitutionName { get; set; } = "PROCOMER";
    public string? LogoPath { get; set; }
}

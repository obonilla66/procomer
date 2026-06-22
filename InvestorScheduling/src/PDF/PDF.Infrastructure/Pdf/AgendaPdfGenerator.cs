/*
 * Generador del documento PDF profesional.
 *
 * Usa QuestPDF para construir:
 * - Encabezado institucional.
 * - Datos del inversor.
 * - Fecha de la jornada.
 * - Tabla de reuniones.
 * - Indicadores de traslado.
 * - Pie de página con fecha de generación y numeración.
 *
 * Si existe un logo físico en la ruta configurada, se usa.
 * Si no existe, se genera un bloque textual institucional.
 */

using Microsoft.Extensions.Options;
using PDF.Application.Interfaces;
using PDF.Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PDF.Infrastructure.Pdf;

public class AgendaPdfGenerator : IPdfGenerator
{
    private readonly PdfOptions _options;

    public AgendaPdfGenerator(IOptions<PdfOptions> options)
    {
        _options = options.Value;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] GenerarAgendaPdf(AgendaPdfData agenda)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(35);
                page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, agenda));
                page.Content().Element(c => ComposeContent(c, agenda));
                page.Footer().Element(ComposeFooter);
            });
        }).GeneratePdf();
    }

    private void ComposeHeader(IContainer container, AgendaPdfData agenda)
    {
        container.Column(column =>
        {
            column.Item().Row(row =>
            {
                row.ConstantItem(90).Height(55).Element(ComposeLogo);

                row.RelativeItem().Column(info =>
                {
                    info.Item().Text("Agenda de reuniones para inversionista")
                        .FontSize(18)
                        .Bold()
                        .FontColor(Colors.Blue.Darken3);

                    info.Item().Text($"Inversor: {agenda.NombreInversor}")
                        .FontSize(12)
                        .SemiBold();

                    if (!string.IsNullOrWhiteSpace(agenda.EmpresaRepresenta))
                    {
                        info.Item().Text($"Empresa: {agenda.EmpresaRepresenta}");
                    }

                    info.Item().Text($"País de origen: {agenda.PaisOrigen}");
                });
            });

            column.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
        });
    }

    private void ComposeLogo(IContainer container)
    {
        var logoPath = _options.LogoPath;

        if (!string.IsNullOrWhiteSpace(logoPath) && File.Exists(logoPath))
        {
            container.Image(logoPath).FitArea();
            return;
        }

        container
            .Border(1)
            .BorderColor(Colors.Blue.Darken3)
            .Background(Colors.Blue.Lighten5)
            .AlignCenter()
            .AlignMiddle()
            .Text(_options.InstitutionName)
            .FontSize(12)
            .Bold()
            .FontColor(Colors.Blue.Darken3);
    }

    private void ComposeContent(IContainer container, AgendaPdfData agenda)
    {
        container.PaddingTop(15).Column(column =>
        {
            column.Spacing(10);

            column.Item().Text($"Fecha de la jornada: {agenda.Fecha:dddd dd 'de' MMMM 'de' yyyy}")
                .FontSize(12)
                .Bold();

            column.Item().Text("Detalle de reuniones")
                .FontSize(13)
                .Bold()
                .FontColor(Colors.Blue.Darken3);

            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(55);
                    columns.ConstantColumn(55);
                    columns.RelativeColumn(1.4f);
                    columns.RelativeColumn(1.2f);
                    columns.RelativeColumn(1.5f);
                    columns.RelativeColumn(1.8f);
                    columns.ConstantColumn(55);
                });

				table.Header(header =>
				{
					header.Cell().Element(c => HeaderCell(c, "Inicio"));
					header.Cell().Element(c => HeaderCell(c, "Fin"));
					header.Cell().Element(c => HeaderCell(c, "Participante"));
					header.Cell().Element(c => HeaderCell(c, "Cargo"));
					header.Cell().Element(c => HeaderCell(c, "Oficina"));
					header.Cell().Element(c => HeaderCell(c, "Dirección"));
					header.Cell().Element(c => HeaderCell(c, "Idioma"));
				});

                foreach (var detalle in agenda.Detalles)
                {
                    BodyCell(table, detalle.FechaHoraInicio.ToString("HH:mm"));
                    BodyCell(table, detalle.FechaHoraFin.ToString("HH:mm"));
                    BodyCell(table, detalle.NombreParticipante);
                    BodyCell(table, detalle.Cargo ?? "-");
                    BodyCell(table, detalle.Oficina);
                    BodyCell(table, detalle.Direccion);
                    BodyCell(table, $"{detalle.Idioma} ({detalle.CodigoIdioma})");

                    if (detalle.TiempoTrasladoSiguienteOficinaMinutos.HasValue)
                    {
                        table.Cell().ColumnSpan(7)
                            .PaddingVertical(4)
                            .PaddingHorizontal(6)
                            .Background(Colors.Grey.Lighten4)
                            .Text($"Traslado estimado hacia la siguiente reunión: {detalle.TiempoTrasladoSiguienteOficinaMinutos.Value} minutos.")
                            .Italic()
                            .FontSize(8);
                    }
                }
            });
        });
    }

	private static void HeaderCell(IContainer container, string text)
	{
		container
			.Background(Colors.Blue.Darken3)
			.Padding(5)
			.Text(text)
			.FontColor(Colors.White)
			.Bold()
			.FontSize(8);
	}

    private static void BodyCell(TableDescriptor table, string text)
    {
        table.Cell()
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten2)
            .Padding(5)
            .Text(text)
            .FontSize(8);
    }

    private static void ComposeFooter(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem()
                .Text($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}")
                .FontSize(8)
                .FontColor(Colors.Grey.Darken1);

            row.ConstantItem(120)
                .AlignRight()
                .Text(text =>
                {
                    text.Span("Página ").FontSize(8);
                    text.CurrentPageNumber().FontSize(8);
                    text.Span(" de ").FontSize(8);
                    text.TotalPages().FontSize(8);
                });
        });
    }
}

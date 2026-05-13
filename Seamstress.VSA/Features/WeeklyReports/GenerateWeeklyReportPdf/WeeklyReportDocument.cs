using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Seamstress.VSA.Domain.Entities;
using Seamstress.VSA.Infrastructure.Extensions;

namespace Seamstress.VSA.Features.WeeklyReports.GenerateWeeklyReportPdf;

public class WeeklyReportDocument(WeeklyReport report) : IDocument
{
    private readonly WeeklyReport _report = report;

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            // Configuración de página
            page.Size(PageSizes.A4);
            page.Margin(40);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().Element(ComposeFooter);
        });
    }

    // ── HEADER ──────────────────────────────────────────────────────────────
    private void ComposeHeader(IContainer container)
    {
        var logoBytes = EmbeddedResourceProvider.GetLogoBytes();

        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                row.ConstantItem(80).Height(80).Image(logoBytes).FitArea();

                row.ConstantItem(16);

                row.RelativeItem().AlignMiddle().Column(inner =>
                {
                    inner.Item()
                        .Text("Confecciones Margarita")
                        .FontSize(9)
                        .FontColor(Color.FromHex("#6b7280"))
                        .Italic();

                    inner.Item()
                        .Text("Reporte Semanal")
                        .FontSize(22)
                        .Bold()
                        .FontColor(Color.FromHex("#1a1a2e"));

                    inner.Item()
                        .Text("Taller — Tela, Aguja e Hilo")
                        .FontSize(10)
                        .FontColor(Color.FromHex("#6b7280"));
                });

                row.ConstantItem(130).AlignMiddle().AlignRight().Column(inner =>
                {
                    inner.Item()
                        .Text("Generado el")
                        .FontSize(9)
                        .FontColor(Color.FromHex("#6b7280"));

                    inner.Item()
                        .Text(DateTimeOffset.UtcNow
                            .ToOffset(TimeSpan.FromHours(-6))
                            .ToString("dd/MM/yyyy"))
                        .FontSize(11)
                        .Bold()
                        .FontColor(Color.FromHex("#1a1a2e"));

                    inner.Item()
                        .Text(DateTimeOffset.UtcNow
                            .ToOffset(TimeSpan.FromHours(-6))
                            .ToString("HH:mm") + " hrs")
                        .FontSize(9)
                        .FontColor(Color.FromHex("#6b7280"));
                });
            });

            col.Item().PaddingTop(10).LineHorizontal(2).LineColor(Color.FromHex("#b8960c"));
        });
    }

    // ── CONTENT ─────────────────────────────────────────────────────────────
    private void ComposeContent(IContainer container)
    {
        container.PaddingTop(20).Column(col =>
        {
            // Período del reporte
            col.Item().Element(ComposePeriodSection);

            col.Item().PaddingTop(24).Element(ComposeFinancialSummary);

            col.Item().PaddingTop(24).Element(ComposeBreakdown);
        });
    }

    private void ComposePeriodSection(IContainer container)
    {
        // Calcular inicio de semana desde CreatedAt del reporte
        var createdLocal = _report.CreatedAt.ToOffset(TimeSpan.FromHours(-6));

        container.Background(Color.FromHex("#f0f4ff"))
            .Padding(16)
            .Column(col =>
            {
                col.Item()
                    .Text("Período del Reporte")
                    .FontSize(10)
                    .FontColor(Color.FromHex("#6b7280"));

                col.Item().PaddingTop(4)
                    .Text($"Semana generada el {createdLocal:dddd, dd 'de' MMMM 'de' yyyy}")
                    .FontSize(13)
                    .Bold()
                    .FontColor(Color.FromHex("#1a1a2e"));
            });
    }

    private void ComposeFinancialSummary(IContainer container)
    {
        container.Column(col =>
        {
            col.Item()
                .Text("Resumen Financiero")
                .FontSize(14)
                .Bold()
                .FontColor(Color.FromHex("#1a1a2e"));

            col.Item().PaddingTop(12).Row(row =>
            {
                // Tarjeta: Ingreso Total
                row.RelativeItem().Element(c =>
                    SummaryCard(c, "Ingreso Total", _report.TotalIncome, "#16a34a"));

                row.ConstantItem(12); // Espaciado

                // Tarjeta: ISR (10%)
                row.RelativeItem().Element(c =>
                    SummaryCard(c, "ISR (10%)", _report.Isr, "#dc2626"));

                row.ConstantItem(12);

                // Tarjeta: Utilidad Neta
                row.RelativeItem().Element(c =>
                    SummaryCard(c, "Utilidad Neta", _report.NetProfit,
                        _report.NetProfit >= 0 ? "#1d4ed8" : "#dc2626"));
            });
        });
    }

    private static void SummaryCard(
        IContainer container, string label, decimal value, string hexColor)
    {
        container
            .Border(1)
            .BorderColor(Color.FromHex("#e5e7eb"))
            .Background(Colors.White)
            .Padding(16)
            .Column(col =>
            {
                col.Item()
                    .Text(label)
                    .FontSize(10)
                    .FontColor(Color.FromHex("#6b7280"));

                col.Item().PaddingTop(6)
                    .Text($"${value:N2}")
                    .FontSize(18)
                    .Bold()
                    .FontColor(Color.FromHex(hexColor));
            });
    }

    private void ComposeBreakdown(IContainer container)
    {
        container.Column(col =>
        {
            col.Item()
                .Text("Desglose de Gastos")
                .FontSize(14)
                .Bold()
                .FontColor(Color.FromHex("#1a1a2e"));

            col.Item().PaddingTop(12).Table(table =>
            {
                // Definir columnas
                table.ColumnsDefinition(cols =>
                {
                    cols.RelativeColumn(3); // Concepto
                    cols.RelativeColumn(2); // Monto
                    cols.RelativeColumn(2); // % sobre ingreso
                });

                // Header de la tabla
                table.Header(header =>
                {
                    void HeaderCell(string text) =>
                        header.Cell()
                            .Background(Color.FromHex("#1a1a2e"))
                            .Padding(10)
                            .Text(text)
                            .FontSize(10)
                            .Bold()
                            .FontColor(Colors.White);

                    HeaderCell("Concepto");
                    HeaderCell("Monto");
                    HeaderCell("% sobre Ingreso");
                });

                // Filas de datos
                void DataRow(string concept, decimal amount, bool isAlternate = false)
                {
                    var bg = isAlternate ? Color.FromHex("#f9fafb") : Colors.White;
                    var pct = _report.TotalIncome > 0
                        ? (amount / _report.TotalIncome * 100)
                        : 0m;

                    table.Cell().Background(bg).Padding(10)
                        .Text(concept).FontSize(10);

                    table.Cell().Background(bg).Padding(10)
                        .Text($"${amount:N2}").FontSize(10);

                    table.Cell().Background(bg).Padding(10)
                        .Text($"{pct:N1}%").FontSize(10)
                        .FontColor(Color.FromHex("#6b7280"));
                }

                DataRow("Gastos de Empleados", _report.TotalEmployeesExpense);
                DataRow("Gastos de Insumos", _report.TotalSuppliesExpense, true);
                DataRow("ISR (10% sobre utilidad)", _report.Isr);

                // Fila de total
                var totalGastos = _report.TotalEmployeesExpense
                    + _report.TotalSuppliesExpense
                    + _report.Isr;

                table.Cell()
                    .ColumnSpan(1)
                    .Background(Color.FromHex("#f0f4ff"))
                    .Padding(10)
                    .Text("TOTAL DEDUCCIONES")
                    .FontSize(10)
                    .Bold();

                table.Cell()
                    .Background(Color.FromHex("#f0f4ff"))
                    .Padding(10)
                    .Text($"${totalGastos:N2}")
                    .FontSize(10)
                    .Bold()
                    .FontColor(Color.FromHex("#dc2626"));

                table.Cell()
                    .Background(Color.FromHex("#f0f4ff"))
                    .Padding(10)
                    .Text("");
            });
        });
    }

    // ── FOOTER ──────────────────────────────────────────────────────────────
    private void ComposeFooter(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().LineHorizontal(1).LineColor(Color.FromHex("#e5e7eb"));

            col.Item().PaddingTop(8).Row(row =>
            {
                row.RelativeItem()
                    .Text("Seamtress — Documento generado automáticamente")
                    .FontSize(8)
                    .FontColor(Color.FromHex("#9ca3af"));

                row.ConstantItem(100).AlignRight()
                    .Text(x =>
                    {
                        x.Span("Página ").FontSize(8).FontColor(Color.FromHex("#9ca3af"));
                        x.CurrentPageNumber().FontSize(8).FontColor(Color.FromHex("#9ca3af"));
                        x.Span(" de ").FontSize(8).FontColor(Color.FromHex("#9ca3af"));
                        x.TotalPages().FontSize(8).FontColor(Color.FromHex("#9ca3af"));
                    });
            });
        });
    }
}
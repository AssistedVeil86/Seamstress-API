using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using Seamstress.VSA.Domain.Errors;
using Seamstress.VSA.Infrastructure.Data;
using Seamstress.VSA.Infrastructure.Extensions;

namespace Seamstress.VSA.Features.WeeklyReports.GenerateWeeklyReportPdf;

public static class GenerateWeeklyReportPdfEndpoint
{
    public static RouteGroupBuilder MapCreateWeeklyReportPdfEndpoint(this RouteGroupBuilder route)
    {
        route.MapGet("{reportId}/pdf", Handler)
            .WithSummary("Generar PDF del Reporte Semanal")
            .WithDescription("Descarga el PDF del reporte semanal por su ID")
            .Produces(StatusCodes.Status200OK, contentType: "application/pdf")
            .ProducesProblem(StatusCodes.Status404NotFound);

        return route;
    }

    private static async Task<Results<FileContentHttpResult, ProblemHttpResult>> Handler(
        int reportId, GenerateWeeklyReportPdfHandler handler, CancellationToken ct)
    {
        var result = await handler.HandleAsync(reportId, ct);

        return result.Match<Results<FileContentHttpResult, ProblemHttpResult>>(
            report => TypedResults.File(report.Bytes,contentType: "application/pdf",report.Filename),
            errors => errors.ToProblemResult()
        );
    }
}

internal sealed class GenerateWeeklyReportPdfHandler(AppDbContext context)
{
    public async Task<ErrorOr<WeeklyReportFileResponse>> HandleAsync(
        int reportId, CancellationToken ct)
    {
        var report = await context.WeeklyReports
            .AsNoTracking()
            .FirstOrDefaultAsync(wr => wr.Id == reportId, ct);

        if (report is null)
            return WeeklyReportErrors.WeeklyReportNotFound($"Report with {reportId} not found");

        var document = new WeeklyReportDocument(report);
        var pdfBytes = document.GeneratePdf();

        var localDate = report.CreatedAt
            .ToOffset(TimeSpan.FromHours(-6))
            .ToString("yyyy-MM-dd");

        var fileName = $"reporte-semanal-{localDate}.pdf";

        return new WeeklyReportFileResponse(pdfBytes, fileName);
    }
}

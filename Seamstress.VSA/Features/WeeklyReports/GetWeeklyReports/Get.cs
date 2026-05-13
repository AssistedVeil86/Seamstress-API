using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Seamstress.VSA.Domain.Shared;
using Seamstress.VSA.Infrastructure.Data;
using Seamstress.VSA.Infrastructure.Extensions;

namespace Seamstress.VSA.Features.WeeklyReports.GetWeeklyReports;

public static class GetWeeklyReportsEndpoint
{
    public static RouteGroupBuilder MapGetWeeklyReportsEndpoint(this RouteGroupBuilder route)
    {
        route.MapGet("/", Handler)
            .WithSummary("Obtener los Reportes Semanales")
            .WithDescription("Obtener los Reportes Semanales paginados de 4 en 4")
            .WithRequestValidation<GetWeeklyReportRequest>();
            
        return route;
    }

    private static async Task<Ok<PagedResponse<WeeklyReportResponse>>> Handler(
        [AsParameters] GetWeeklyReportRequest req,
        GetWeeklyReportsHandler handler,
        CancellationToken ct)
    {
        var result = await handler.HandleAsync(req, ct);
        return TypedResults.Ok(result);
    }
}

internal sealed class GetWeeklyReportsHandler(AppDbContext context)
{
    public async Task<PagedResponse<WeeklyReportResponse>> HandleAsync(
        GetWeeklyReportRequest req, CancellationToken ct
    )
    {
        var baseQuery = context.WeeklyReports
            .AsNoTracking();

        if (req.Month.HasValue && req.Year.HasValue)
        {
            var (monthStart, monthEnd) = DateTimeExtensions.GetMonthRangeUtc(
                req.Month.Value, req.Year.Value);

            baseQuery = baseQuery.Where(wr =>
                wr.CreatedAt >= monthStart &&
                wr.CreatedAt < monthEnd);
        }

        var totalCount = await baseQuery.CountAsync(ct);

        var WeeklyReports = await baseQuery
            .Skip((req.Page - 1) * req.Size)
            .Take(req.Size)
            .Select(wr => wr.ToResponse())
            .ToListAsync(ct);

        return PagedResponse<WeeklyReportResponse>.Create(WeeklyReports, totalCount,
            req.Page, req.Size);
    }
}

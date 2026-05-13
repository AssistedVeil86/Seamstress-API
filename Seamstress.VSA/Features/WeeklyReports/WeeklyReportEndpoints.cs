using System;
using Seamstress.VSA.Features.WeeklyReports.GenerateWeeklyReportPdf;
using Seamstress.VSA.Features.WeeklyReports.GetWeeklyReports;

namespace Seamstress.VSA.Features.WeeklyReports;

public static class WeeklyReportEndpoints
{
    public static IServiceCollection AddWeeklyReportHandlers(this IServiceCollection services)
    {
        services.AddScoped<GetWeeklyReportsHandler>();
        services.AddScoped<GenerateWeeklyReportPdfHandler>();
        return services;
    }

    public static WebApplication MapWeeklyReportEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/weeklyReports")
            .WithTags("Weekly Reports")
            .RequireAuthorization();

        group.MapGetWeeklyReportsEndpoint();
        group.MapCreateWeeklyReportPdfEndpoint();

        return app;
    }
}

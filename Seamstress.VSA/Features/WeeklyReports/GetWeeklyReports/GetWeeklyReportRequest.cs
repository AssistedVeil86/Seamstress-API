using System;

namespace Seamstress.VSA.Features.WeeklyReports.GetWeeklyReports;

public sealed record GetWeeklyReportRequest(
    int? Month,
    int? Year,
    int Page = 1,
    int Size = 4);

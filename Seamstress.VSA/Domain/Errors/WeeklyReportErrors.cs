using System;
using ErrorOr;

namespace Seamstress.VSA.Domain.Errors;

public static class WeeklyReportErrors
{
    public static Error WeeklyReportNotFound(string message) =>
        Error.NotFound("WeeklyReport.NotFound", message);
}

namespace Seamstress.VSA.Features.WeeklyReports.GetWeeklyReports;

public sealed record class WeeklyReportResponse(
    int Id,
    decimal TotalIncome,
    decimal Isr,
    decimal TotalEmployeesExpense,
    decimal TotalSuppliesExpense,
    decimal NetProfit,
    DateTimeOffset CreatedAt);

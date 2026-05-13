namespace Seamstress.VSA.Features.Expenses.GetExpenses;

public sealed record GetExpensesRequest(
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    int Page = 1,
    int Size = 10);

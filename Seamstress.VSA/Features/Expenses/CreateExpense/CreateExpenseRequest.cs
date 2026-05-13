using Seamstress.VSA.Domain.Enums;

namespace Seamstress.VSA.Features.Expenses.CreateExpense;

public sealed record CreateExpenseRequest(string Detail, decimal Amount, ExpenseType Type);

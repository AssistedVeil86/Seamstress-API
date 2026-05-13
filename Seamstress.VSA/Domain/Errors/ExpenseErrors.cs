using System;
using ErrorOr;

namespace Seamstress.VSA.Domain.Errors;

public class ExpenseErrors
{
    public static Error ExpenseNotFound(string message) =>
        Error.NotFound("Expense.NotFound", message);
}

using System;
using Seamstress.VSA.Domain.Enums;

namespace Seamstress.VSA.Features.Expenses.Shared;

public sealed record ExpenseResponse(
    int Id,
    string Detail,
    decimal Amount,
    ExpenseType Type,
    DateTimeOffset CreatedAt);

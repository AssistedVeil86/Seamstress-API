using System;
using Seamstress.VSA.Domain.Enums;

namespace Seamstress.VSA.Domain.Entities;

public class Expense : BaseEntity
{
    public string Detail { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public ExpenseType Type { get; set; }
}

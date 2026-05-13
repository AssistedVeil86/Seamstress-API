using System;

namespace Seamstress.VSA.Domain.Entities;

public class WeeklyReport : BaseEntity
{
    public decimal TotalIncome { get; set; }
    public decimal Isr { get; set; }
    public decimal TotalEmployeesExpense { get; set; }
    public decimal TotalSuppliesExpense { get; set; }
    public decimal NetProfit { get; set; }
}

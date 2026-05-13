using Microsoft.EntityFrameworkCore;
using Seamstress.VSA.Domain.Entities;
using Seamstress.VSA.Domain.Enums;
using Seamstress.VSA.Infrastructure.Data;
using Seamstress.VSA.Infrastructure.Extensions;

namespace Seamstress.VSA.Infrastructure.BackgroundJobs.Implementation;

public class WeeklyReportJob(
    AppDbContext context,
    ILogger<WeeklyReportJob> logger
) : IWeeklyReportJob
{
    public async Task ExecuteAsync()
    {
        var (utcStart, utcEnd) = DateTimeExtensions.GetCurrentWeekUtcRange();

        logger.LogInformation(
            "Rango UTC calculado: {Start} → {End}",
            utcStart, utcEnd);

        var existingReport = await context.WeeklyReports
            .AnyAsync(wr => wr.CreatedAt >= utcStart && wr.CreatedAt < utcEnd);

        if (existingReport)
        {
            logger.LogWarning(
                "Ya existe un reporte para la semana {Start} → {End}. Se omite la ejecución.",
                utcStart, utcEnd);
            return;
        }

        var orders = await context.Orders
            .AsNoTracking()
            .Where(o => o.CreatedAt >= utcStart && o.CreatedAt < utcEnd
                && o.Status == OrderStatus.COMPLETED)
            .ToListAsync();

        var expenses = await context.Expenses
            .AsNoTracking()
            .Where(e => e.CreatedAt >= utcStart && e.CreatedAt < utcEnd)
            .ToListAsync();

        if (orders.Count == 0 && expenses.Count == 0)
        {
            logger.LogWarning(
                "Sin órdenes ni gastos para {Start} → {End}. No se genera reporte.",
                utcStart, utcEnd);
            return;
        }

        var totalIncome = orders.Sum(o => o.Total ?? 0m);

        var employeesExpense = expenses.Where(e => e.Type == ExpenseType.EMPLOYEES)
            .Sum(e => e.Amount);

        var suppliesExpense = expenses.Where(e => e.Type == ExpenseType.SUPPLIES)
            .Sum(e => e.Amount);

        var totalExpenses = employeesExpense + suppliesExpense;
    
        var isr = totalIncome > 0 ? Math.Round(totalIncome * 0.10m, 2) : 0m;
        var netProfit = totalIncome - isr - totalExpenses;

        if (netProfit < 0)
        {
            logger.LogWarning(
                "Semana con pérdida neta: {NetProfit}. Income: {Income}, Gastos: {Expenses}",
                netProfit, totalIncome, totalExpenses);
        }

        var report = new WeeklyReport
        {
            TotalIncome = totalIncome,
            Isr = isr,
            NetProfit = netProfit,
            TotalEmployeesExpense = employeesExpense,
            TotalSuppliesExpense = suppliesExpense
        };

        context.WeeklyReports.Add(report);
        await context.SaveChangesAsync();

        logger.LogInformation("Reporte guardado exitosamente.");
    }
}

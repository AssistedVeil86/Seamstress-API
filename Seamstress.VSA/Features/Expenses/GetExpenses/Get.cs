using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Seamstress.VSA.Domain.Shared;
using Seamstress.VSA.Features.Expenses.Shared;
using Seamstress.VSA.Infrastructure.Data;
using Seamstress.VSA.Infrastructure.Extensions;

namespace Seamstress.VSA.Features.Expenses.GetExpenses;

public static class GetExpensesEndpoint
{
    public static RouteGroupBuilder MapGetExpensesEndpoint(this RouteGroupBuilder route)
    {
        route.MapGet("/", Handler)
            .WithSummary("Obtener todos los gastos")
            .WithDescription("Obtener todos los gastos de la semana paginados de 10 en 10")
            .WithRequestValidation<GetExpensesRequest>();

        return route;
    }

    private static async Task<Ok<PagedResponse<ExpenseResponse>>> Handler(
        [AsParameters] GetExpensesRequest req, GetExpensesHandler handler, CancellationToken ct)
    {
        var result = await handler.HandleAsync(req, ct);
        return TypedResults.Ok(result);
    }
}

internal sealed class GetExpensesHandler(AppDbContext context)
{
    public async Task<PagedResponse<ExpenseResponse>> HandleAsync(
        GetExpensesRequest req, CancellationToken ct)
    {
        var (startUtc, endUtc) = req.StartDate.ToUtcRange(req.EndDate);

        var baseQuery = context.Expenses
            .AsNoTracking()
            .Where(e => e.CreatedAt >= startUtc && e.CreatedAt < endUtc)
            .OrderByDescending(e => e.CreatedAt);

        var totalCount = await baseQuery.CountAsync(ct);

        var expenses = await baseQuery
            .Skip((req.Page - 1) * req.Size)
            .Take(req.Size)
            .Select(e => e.ToResponse())
            .ToListAsync();

        return PagedResponse<ExpenseResponse>.Create(expenses, totalCount, req.Page, req.Size);
    }
}

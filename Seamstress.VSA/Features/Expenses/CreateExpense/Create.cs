using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Seamstress.VSA.Domain.Entities;
using Seamstress.VSA.Features.Expenses.Shared;
using Seamstress.VSA.Infrastructure.Data;
using Seamstress.VSA.Infrastructure.Extensions;

namespace Seamstress.VSA.Features.Expenses.CreateExpense;

public static class CreateExpenseEndpoint
{
    public static RouteGroupBuilder MapCreateExpenseEndpoint(this RouteGroupBuilder route)
    {
        route.MapPost("/", Handler)
            .WithSummary("Crear un Gasto junto a sus detalles.")
            .WithDescription("Crear un Gasto de acuerdo a su tipo, cantidad del gasto, y detalle")
            .WithRequestValidation<CreateExpenseRequest>();
            
        return route;
    }

    private static async Task<Created<ExpenseResponse>> Handler(
        CreateExpenseRequest req, CreateExpenseHandler handler, CancellationToken ct)
    {
        var result = await handler.HandleAsync(req, ct);
        return TypedResults.Created($"api/expenses/{result.Id}", result);
    }
}

internal sealed class CreateExpenseHandler(AppDbContext context)
{
    public async Task<ExpenseResponse> HandleAsync(
        CreateExpenseRequest req, CancellationToken ct)
    {
        var expense = new Expense()
        {
            Detail = req.Detail,
            Amount = req.Amount,
            Type = req.Type
        };

        var createdExpense = context.Expenses.Add(expense).Entity;
        await context.SaveChangesAsync(ct);

        return createdExpense.ToResponse();
    }
}

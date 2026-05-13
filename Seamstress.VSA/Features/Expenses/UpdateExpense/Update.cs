using System;
using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;
using Seamstress.VSA.Domain.Errors;
using Seamstress.VSA.Features.Expenses.Shared;
using Seamstress.VSA.Infrastructure.Data;
using Seamstress.VSA.Infrastructure.Extensions;

namespace Seamstress.VSA.Features.Expenses.UpdateExpense;

public static class UpdateExpenseEndpoint
{
    public static RouteGroupBuilder MapUpdateExpenseEndpoint(this RouteGroupBuilder route)
    {
        route.MapPut("{expenseId}", Handler)
            .WithSummary("Actualizar un Gasto segun su Id")
            .WithDescription("Actualizar el detalle o cantidad del gasto.")
            .WithRequestValidation<UpdateExpenseRequest>()
            .ProducesProblem(StatusCodes.Status404NotFound);
        
        return route;
    }

    private static async Task<Results<Ok<ExpenseResponse>, ProblemHttpResult>> Handler(
        int expenseId, UpdateExpenseRequest req, UpdateExpenseHandler handler, CancellationToken ct)
    {
        var result = await handler.HandleAsync(expenseId, req, ct);

        return result.Match<Results<Ok<ExpenseResponse>, ProblemHttpResult>>(
            expense => TypedResults.Ok(expense),
            errors => errors.ToProblemResult()
        );
    }
}

internal sealed class UpdateExpenseHandler(AppDbContext context)
{
    public async Task<ErrorOr<ExpenseResponse>> HandleAsync(
        int expenseId, UpdateExpenseRequest req, CancellationToken ct)
    {
        var expense = await context.Expenses.FindAsync([expenseId], ct);

        if (expense is null)
            return ExpenseErrors.ExpenseNotFound($"Expense with Id {expenseId} not found");

        expense.Detail = req.Detail;
        expense.Amount = req.Amount;

        expense.UpdateModified();

        await context.SaveChangesAsync(ct);

        return expense.ToResponse();
    }
}
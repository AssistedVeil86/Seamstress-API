using Seamstress.VSA.Features.Expenses.CreateExpense;
using Seamstress.VSA.Features.Expenses.GetExpenses;
using Seamstress.VSA.Features.Expenses.UpdateExpense;

namespace Seamstress.VSA.Features.Expenses;

public static class ExpenseEndpoints
{
    public static IServiceCollection AddExpenseHandlers(this IServiceCollection services)
    {
        services.AddScoped<CreateExpenseHandler>();
        services.AddScoped<GetExpensesHandler>();
        services.AddScoped<UpdateExpenseHandler>();

        return services;
    }

    public static WebApplication MapExpenseEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/expenses")
            .WithTags("Expenses")
            .RequireAuthorization();

        group.MapCreateExpenseEndpoint();
        group.MapGetExpensesEndpoint();
        group.MapUpdateExpenseEndpoint();

        return app;
    }
}

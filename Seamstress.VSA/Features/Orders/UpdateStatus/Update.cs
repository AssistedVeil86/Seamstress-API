using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;
using Seamstress.VSA.Domain.Errors;
using Seamstress.VSA.Features.Orders.Shared;
using Seamstress.VSA.Infrastructure.Data;
using Seamstress.VSA.Infrastructure.Extensions;

namespace Seamstress.VSA.Features.Orders.UpdateStatus;

public static class UpdateStatusEndpoint
{
    public static RouteGroupBuilder MapUpdateStatusEndpoint(this RouteGroupBuilder route)
    {
        route.MapPut("{orderId}/status", Handler)
            .WithSummary("Actualizar el estado de la orden")
            .WithDescription("Actualizar el estado de la orden a COMPLETADA, al igual que agregar el total de la misma.")
            .WithRequestValidation<UpdateOrderRequest>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return route;
    }

    private static async Task<Results<Ok<OrderResponse>, ProblemHttpResult>> Handler(
        UpdateOrderRequest req, int orderId, UpdateStatusHandler handler, CancellationToken ct)
    {
        var result = await handler.HandleAsync(req, orderId, ct);

        return result.Match<Results<Ok<OrderResponse>, ProblemHttpResult>>(
            order => TypedResults.Ok(order),
            errors => errors.ToProblemResult()
        );
    }
}

internal sealed class UpdateStatusHandler(AppDbContext context)
{
    public async Task<ErrorOr<OrderResponse>> HandleAsync(UpdateOrderRequest req,
        int orderId, CancellationToken ct)
    {
        var order = await context.Orders.FindAsync([orderId], ct);

        if (order is null)
            return OrderErrors.OrderNotFound($"Order with Id {orderId} not found");

        order.Total = req.Total;
        order.Status = req.Status;

        order.UpdateModified();
        await context.SaveChangesAsync(ct);

        return order.ToResponse();
    }
}
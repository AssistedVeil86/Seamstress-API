using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;
using Seamstress.VSA.Domain.Enums;
using Seamstress.VSA.Domain.Errors;
using Seamstress.VSA.Features.Orders.Shared;
using Seamstress.VSA.Infrastructure.Data;
using Seamstress.VSA.Infrastructure.Extensions;

namespace Seamstress.VSA.Features.Orders.UpdateTotal;

public static class UpdateTotalEndpoint
{
    public static RouteGroupBuilder MapUpdateTotalEndpoint(this RouteGroupBuilder route)
    {
        route.MapPut("{orderId}/total", Handler)
            .WithSummary("Actualizar el total de una Orden")
            .WithDescription("Actualizar el estado de una orden una vez esté completada.")
            .WithRequestValidation<UpdateTotalRequest>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        return route;
    }

    private static async Task<Results<Ok<OrderResponse>, ProblemHttpResult>> Handler(
        UpdateTotalRequest req, int orderId, UpdateTotalHandler handler, CancellationToken ct)
    {
        var result = await handler.HandleAsync(req, orderId, ct);
        
        return result.Match<Results<Ok<OrderResponse>, ProblemHttpResult>>(
            order => TypedResults.Ok(order),
            errors => errors.ToProblemResult()
        );
    }
}

internal sealed class UpdateTotalHandler(AppDbContext context)
{
    public async Task<ErrorOr<OrderResponse>> HandleAsync(UpdateTotalRequest req,
        int orderId, CancellationToken ct)
    {
        var order = await context.Orders.FindAsync([orderId], ct);

        if (order is null)
            return OrderErrors.OrderNotFound($"Order with Id {orderId} not found");

        if (order.Status != OrderStatus.COMPLETED)
            return OrderErrors.InvalidOrderStatus($"Order's Status is not COMPLETED");

        order.Total = req.Total;
        order.UpdateModified();

        await context.SaveChangesAsync(ct);

        return order.ToResponse();
    }
}

using Microsoft.AspNetCore.Http.HttpResults;
using Seamstress.VSA.Domain.Enums;
using Seamstress.VSA.Features.Orders.Shared;
using Seamstress.VSA.Infrastructure.Data;
using Seamstress.VSA.Infrastructure.Extensions;
using Seamstress.VSA.Domain.Entities;

namespace Seamstress.VSA.Features.Orders.CreateOrder;

public static class CreateOrderEndpoint
{
    public static RouteGroupBuilder MapCreateOrderEndpoint(this RouteGroupBuilder route)
    {
        route.MapPost("/", Handler)
            .WithSummary("Crear una Orden junto a sus detalles.")
            .WithDescription("Crear una Orden junto a sus detalles, items, y materiales")
            .WithRequestValidation<CreateOrderRequest>();
            
        return route;
    }

    private static async Task<Created<OrderResponse>> Handler(CreateOrderRequest req,
        CreateOrderHandler handler, CancellationToken ct)
    {
        var result = await handler.HandleAsync(req, ct);
        return TypedResults.Created($"api/orders/{result.OrderId}", result);
    }
}

internal sealed class CreateOrderHandler(AppDbContext context)
{
    public async Task<OrderResponse> HandleAsync(CreateOrderRequest req, CancellationToken ct)
    {
        var order = new Order()
        {
            Name = req.Name,
            DueDate = req.DueDate,
            Status = OrderStatus.PENDING,
            Materials = req.Materials.ToEntityList(),
            Details = req.Details.ToEntityList(),
            OrderItems = req.OrderItems.ToEntityList()
        };

        var createdOrder = context.Orders.Add(order).Entity;
        await context.SaveChangesAsync(ct);

        return createdOrder.ToResponse();
    }
}

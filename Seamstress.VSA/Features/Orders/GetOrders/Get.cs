using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Seamstress.VSA.Domain.Shared;
using Seamstress.VSA.Infrastructure.Data;
using Seamstress.VSA.Infrastructure.Extensions;

namespace Seamstress.VSA.Features.Orders.GetOrders;

public static class GetOrdersEndpoint
{
    public static RouteGroupBuilder MapGetOrdersEndpoint(this RouteGroupBuilder route)
    {
        route.MapGet("/", Handler)
            .WithSummary("Obtener todas las ordenes")
            .WithDescription("Obtener todas las ordenes de la semana paginadas de 10 en 10")
            .WithRequestValidation<GetOrdersRequest>();
            
        return route;
    }

    private static async Task<Ok<PagedResponse<OrderSummaryResponse>>> Handler(
        [AsParameters] GetOrdersRequest req, GetOrdersHandler handler, CancellationToken ct)
    {
        var result = await handler.HandleAsync(req, ct);
        return TypedResults.Ok(result);
    }
}

internal sealed class GetOrdersHandler(AppDbContext context)
{
    public async Task<PagedResponse<OrderSummaryResponse>> HandleAsync(GetOrdersRequest req, CancellationToken ct)
    {
        var (startUtc, endUtc) = req.StartDate.ToUtcRange(req.EndDate);

        var baseQuery = context.Orders
            .AsNoTracking()
            .Where(o => o.CreatedAt >= startUtc && o.CreatedAt <= endUtc)
            .OrderByDescending(o => o.CreatedAt);

        var totalCount = await baseQuery.CountAsync(ct);

        var orders = await baseQuery
            .Include(o => o.OrderItems)
            .Include(o => o.Materials)
            .Include(o => o.Details)
            .AsSplitQuery()
            .Skip((req.Page - 1) * req.Size)
            .Take(req.Size)
            .Select(o => o.ToSummaryResponse())
            .ToListAsync(ct);

        return PagedResponse<OrderSummaryResponse>.Create(orders, totalCount, req.Page, req.Size);
    }
}

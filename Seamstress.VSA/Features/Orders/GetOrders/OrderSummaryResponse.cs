using Seamstress.VSA.Domain.Enums;

namespace Seamstress.VSA.Features.Orders.GetOrders
{
    public sealed record OrderSummaryResponse(
        int OrderId,
        string Name,
        decimal? Total,
        OrderStatus Status,
        DateTimeOffset DueDate,
        DateTimeOffset CreatedAt,
        IReadOnlyList<OrderItemResponse> Items,
        IReadOnlyList<MaterialResponse> Materials,
        IReadOnlyList<DetailResponse> Details);

    public sealed record OrderItemResponse(
        int Id,
        int Quantity,
        string Size);

    public sealed record MaterialResponse(
        int Id,
        string Detail,
        int Quantity);

    public sealed record DetailResponse(
        int Id,
        string Description);
}
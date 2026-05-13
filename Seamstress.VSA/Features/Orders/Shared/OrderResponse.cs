using Seamstress.VSA.Domain.Enums;

namespace Seamstress.VSA.Features.Orders.Shared;

public sealed record OrderResponse(
    int OrderId,
    string Name,
    decimal? Total,
    OrderStatus Status,
    DateTimeOffset DueDate,
    DateTimeOffset CreatedAt);
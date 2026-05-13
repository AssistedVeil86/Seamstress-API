namespace Seamstress.VSA.Features.Orders.GetOrders;

public sealed record GetOrdersRequest(
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    int Page = 1,
    int Size = 10);

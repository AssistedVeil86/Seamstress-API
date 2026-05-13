namespace Seamstress.VSA.Features.Orders.CreateOrder;

public sealed record CreateOrderRequest(
    string Name,
    DateTimeOffset DueDate,
    List<CreateOrderDetailDto> Details,
    List<CreateOrderItemDto> OrderItems,
    List<CreateOrderMaterialDto> Materials
);

public sealed record CreateOrderMaterialDto(string Detail, int Quantity);
public sealed record CreateOrderDetailDto(string Description);
public sealed record CreateOrderItemDto(int Quantity, string Size);
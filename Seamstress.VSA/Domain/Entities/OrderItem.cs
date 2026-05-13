using System;

namespace Seamstress.VSA.Domain.Entities;

public class OrderItem : BaseEntity
{
    public int Quantity { get; set; }
    public string Size { get; set; } = string.Empty;
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}

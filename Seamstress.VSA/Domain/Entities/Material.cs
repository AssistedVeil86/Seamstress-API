using System;

namespace Seamstress.VSA.Domain.Entities;

public class Material : BaseEntity
{
    public string Detail { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}

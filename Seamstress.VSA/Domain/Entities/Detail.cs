using System;

namespace Seamstress.VSA.Domain.Entities;

public class Detail : BaseEntity
{
    public string Description { get; set; } = string.Empty;
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}

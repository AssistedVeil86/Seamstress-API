using System;
using Seamstress.VSA.Domain.Enums;

namespace Seamstress.VSA.Domain.Entities;

public class Order : BaseEntity
{
    public Order()
    {
        Materials = [];
        Details = [];
        OrderItems = [];
    }

    public string Name { get; set; } = string.Empty;
    public decimal? Total { get; set; }
    public DateTimeOffset DueDate { get; set; }
    public OrderStatus Status { get; set; }
    public List<Material> Materials { get; set; }
    public List<Detail> Details { get; set; }
    public List<OrderItem> OrderItems { get; set; }
}
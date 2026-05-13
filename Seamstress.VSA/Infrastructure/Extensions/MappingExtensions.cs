using Seamstress.VSA.Domain.Entities;
using Seamstress.VSA.Features.Expenses.Shared;
using Seamstress.VSA.Features.Orders.CreateOrder;
using Seamstress.VSA.Features.Orders.GetOrders;
using Seamstress.VSA.Features.Orders.Shared;
using Seamstress.VSA.Features.WeeklyReports.GetWeeklyReports;

namespace Seamstress.VSA.Infrastructure.Extensions;

public static class MappingExtensions
{
    public static List<Detail> ToEntityList(this List<CreateOrderDetailDto> detailDtos)
    {
        return detailDtos.Select(d => new Detail
        {
            Description = d.Description
        }).ToList();
    }

    public static List<OrderItem> ToEntityList(this List<CreateOrderItemDto> itemDtos)
    {
        return itemDtos.Select(i => new OrderItem
        {
            Quantity = i.Quantity,
            Size = i.Size
        }).ToList();
    }

    public static List<Material> ToEntityList(this List<CreateOrderMaterialDto> materialDtos)
    {
        return materialDtos.Select(m => new Material
        {
            Detail = m.Detail,
            Quantity = m.Quantity,
        }).ToList();
    }

    private static List<OrderItemResponse> ToResponse(this List<OrderItem> orderItems)
    {
        return orderItems
        .Select(o => new OrderItemResponse(o.Id, o.Quantity, o.Size))
        .ToList();
    }

    private static List<DetailResponse> ToResponse(this List<Detail> details)
    {
        return details
        .Select(d => new DetailResponse(d.Id, d.Description))
        .ToList();
    }

    private static List<MaterialResponse> ToResponse(this List<Material> materials)
    {
        return materials
        .Select(m => new MaterialResponse(m.Id, m.Detail, m.Quantity))
        .ToList();
    }

    public static OrderResponse ToResponse(this Order order)
    {
        return new OrderResponse(
            order.Id,
            order.Name,
            order.Total,
            order.Status,
            order.DueDate,
            order.CreatedAt);
    }

    public static OrderSummaryResponse ToSummaryResponse(this Order order)
    {
        return new OrderSummaryResponse(
            order.Id,
            order.Name,
            order.Total,
            order.Status,
            order.DueDate,
            order.CreatedAt,
            order.OrderItems.ToResponse(),
            order.Materials.ToResponse(),
            order.Details.ToResponse());
    }

    public static WeeklyReportResponse ToResponse(this WeeklyReport weeklyReport)
    {
        return new WeeklyReportResponse(
            weeklyReport.Id,
            weeklyReport.TotalIncome,
            weeklyReport.Isr,
            weeklyReport.TotalEmployeesExpense,
            weeklyReport.TotalSuppliesExpense,
            weeklyReport.NetProfit,
            weeklyReport.CreatedAt);
    }

    public static ExpenseResponse ToResponse(this Expense expense)
    {
        return new ExpenseResponse(
            expense.Id,
            expense.Detail,
            expense.Amount,
            expense.Type,
            expense.CreatedAt);
    }
}
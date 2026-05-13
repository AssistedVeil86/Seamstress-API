using Seamstress.VSA.Features.Orders.CreateOrder;
using Seamstress.VSA.Features.Orders.GetOrders;
using Seamstress.VSA.Features.Orders.UpdateStatus;
using Seamstress.VSA.Features.Orders.UpdateTotal;

namespace Seamstress.VSA.Features.Orders;

public static class OrderEndpoints
{
    public static IServiceCollection AddOrderHandlers(this IServiceCollection services)
    {
        services.AddScoped<CreateOrderHandler>();
        services.AddScoped<UpdateStatusHandler>();
        services.AddScoped<GetOrdersHandler>();
        services.AddScoped<UpdateTotalHandler>();

        return services;
    }

    public static WebApplication MapOrderEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/orders")
            .WithTags("Orders")
            .RequireAuthorization();

        group.MapCreateOrderEndpoint();
        group.MapUpdateStatusEndpoint();
        group.MapUpdateTotalEndpoint();
        group.MapGetOrdersEndpoint();

        return app;
    }
}
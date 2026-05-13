using System;
using ErrorOr;

namespace Seamstress.VSA.Domain.Errors;

public static class OrderErrors
{
    public static Error OrderNotFound(string message) =>
        Error.NotFound("Order.NotFound", message);

    public static Error InvalidOrderStatus(string message) =>
        Error.Failure("Order.InvalidStatus", message);
}

using System;
using Seamstress.VSA.Domain.Enums;

namespace Seamstress.VSA.Features.Orders.UpdateStatus;

public sealed record UpdateOrderRequest(decimal Total, OrderStatus Status);

using System;
using FluentValidation;

namespace Seamstress.VSA.Features.Orders.GetOrders;

public class GetOrdersValidator : AbstractValidator<GetOrdersRequest>
{
    public GetOrdersValidator()
    {
        RuleFor(x => x.EndDate)
            .NotEmpty()
            .GreaterThan(x => x.StartDate)
            .WithMessage("La fecha de fin debe ser mayor a la de inicio.");

        RuleFor(x => x)
            .Must(x => (x.EndDate - x.StartDate).TotalDays <= 7)
            .WithMessage("El rango no puede ser mayor a 7 días.");

        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("La página debe ser mayor a 0.");

        RuleFor(x => x.Size)
            .InclusiveBetween(1, 50)
            .WithMessage("El tamaño de página debe estar entre 1 y 50.");
    }
}

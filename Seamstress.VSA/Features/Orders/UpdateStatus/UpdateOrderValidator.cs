using FluentValidation;
using Seamstress.VSA.Domain.Enums;

namespace Seamstress.VSA.Features.Orders.UpdateStatus;

public class UpdateOrderValidator : AbstractValidator<UpdateOrderRequest>
{
    public UpdateOrderValidator()
    {
        RuleFor(x => x.Total)
            .NotEmpty().WithMessage("El Total de la Orden es requerido")
            .GreaterThan(0).WithMessage("El total de la orden debe ser mayor a 0");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("El Estado de la Orden es requerido")
            .Must(status => status == OrderStatus.COMPLETED);
    }
}

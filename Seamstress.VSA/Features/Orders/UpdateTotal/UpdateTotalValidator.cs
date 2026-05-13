using FluentValidation;

namespace Seamstress.VSA.Features.Orders.UpdateTotal;

public class UpdateTotalValidator : AbstractValidator<UpdateTotalRequest>
{
    public UpdateTotalValidator()
    {
        RuleFor(x => x.Total)
            .NotEmpty().WithMessage("El Total de la Orden es requerido")
            .GreaterThan(0).WithMessage("El total de la orden debe ser mayor a 0");
    }
}

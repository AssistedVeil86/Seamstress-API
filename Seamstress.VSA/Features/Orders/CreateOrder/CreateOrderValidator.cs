using FluentValidation;

namespace Seamstress.VSA.Features.Orders.CreateOrder;

public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre de la Orden es Requerido")
            .MaximumLength(100);

        RuleFor(x => x.DueDate)
            .NotEmpty().WithMessage("La Fecha de Entrega es Requerida")
            .GreaterThan(DateTimeOffset.UtcNow).WithMessage("La Fecha de Entrega debe ser futura");

        RuleFor(x => x.Details)
            .NotEmpty().WithMessage("La Orden debe tener al menos un detalle");

        RuleFor(x => x.OrderItems)
            .NotEmpty().WithMessage("La Orden debe tener al menos un item");

        RuleFor(x => x.Materials)
            .NotEmpty().WithMessage("La Orden debe tener al menos un material adjunto");

        RuleForEach(x => x.Details).SetValidator(new CreateOrderDetailValidator());
        RuleForEach(x => x.OrderItems).SetValidator(new CreateOrderItemValidator());
        RuleForEach(x => x.Materials).SetValidator(new CreateOrderMaterialValidator());
    }
}

public class CreateOrderMaterialValidator : AbstractValidator<CreateOrderMaterialDto>
{
    public CreateOrderMaterialValidator()
    {
        RuleFor(x => x.Detail).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");
    }
}

public class CreateOrderDetailValidator : AbstractValidator<CreateOrderDetailDto>
{
    public CreateOrderDetailValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
    }
}

public class CreateOrderItemValidator : AbstractValidator<CreateOrderItemDto>
{
    public CreateOrderItemValidator()
    {
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.Size).NotEmpty().WithMessage("El tamaño es obligatorio (ej. S, M, L, XL).");
    }
}

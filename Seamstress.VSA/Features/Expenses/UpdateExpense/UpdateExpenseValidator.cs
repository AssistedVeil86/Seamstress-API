using System;
using FluentValidation;

namespace Seamstress.VSA.Features.Expenses.UpdateExpense;

public class UpdateExpenseValidator : AbstractValidator<UpdateExpenseRequest>
{
    public UpdateExpenseValidator()
    {
        RuleFor(x => x.Detail)
            .NotEmpty().WithMessage("El Detalle del Gasto es Requerido");

        RuleFor(x => x.Amount)
            .NotEmpty().WithMessage("La Cantidad del Gasto es requerida")
            .GreaterThan(0).WithMessage("La Cantidad del Gasto debe ser mayor a 0");
    }
}

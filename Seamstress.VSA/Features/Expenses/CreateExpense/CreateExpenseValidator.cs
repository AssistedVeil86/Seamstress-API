using System;
using System.Data;
using FluentValidation;
using Seamstress.VSA.Domain.Enums;

namespace Seamstress.VSA.Features.Expenses.CreateExpense;

public class CreateExpenseValidator : AbstractValidator<CreateExpenseRequest>
{
    public CreateExpenseValidator()
    {
        RuleFor(x => x.Detail)
            .NotEmpty().WithMessage("El Detalle del Gasto es Requerido");

        RuleFor(x => x.Amount)
            .NotEmpty().WithMessage("La Cantidad del Gasto es requerida")
            .GreaterThan(0).WithMessage("La Cantidad del Gasto debe ser mayor a 0");

        RuleFor(x => x.Type)
            .NotNull()
            .IsInEnum();
    }
}

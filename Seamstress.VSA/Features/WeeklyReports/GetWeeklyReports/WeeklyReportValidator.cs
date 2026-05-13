using System;
using FluentValidation;

namespace Seamstress.VSA.Features.WeeklyReports.GetWeeklyReports;

public class WeeklyReportValidator : AbstractValidator<GetWeeklyReportRequest>
{
    public WeeklyReportValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("La página debe ser mayor a 0.");

        RuleFor(x => x.Size)
            .InclusiveBetween(1, 100)
            .WithMessage("El tamaño de página debe estar entre 1 y 50.");

        When(x => x.Month.HasValue, () =>
        {
            RuleFor(x => x.Month!.Value)
                .InclusiveBetween(1, 12)
                .WithMessage("El mes debe estar entre 1 y 12.");
        });

        When(x => x.Year.HasValue, () =>
        {
            RuleFor(x => x.Year!.Value)
                .InclusiveBetween(2000, 2100)
                .WithMessage("El año no es válido.");
        });
    }
}

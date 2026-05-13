using FluentValidation;
using Seamstress.VSA.Infrastructure.Filters;

namespace Seamstress.VSA.Infrastructure.Extensions;

public static class ValidationExtensions
{
    public static IServiceCollection RegisterValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Program>();
        return services;
    }

    public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder builder)
    {
        builder.AddEndpointFilter<ValidationFilter<TRequest>>()
            .ProducesValidationProblem();

        return builder;
    }
}

using System;
using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Seamstress.VSA.Infrastructure.Extensions;

public static class ErrorExtensions
{
    public static ProblemHttpResult ToProblemResult(this List<Error> errors)
    {
        return Problem(errors[0]);
    }

    private static ProblemHttpResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Failure => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        return TypedResults.Problem(
            title: error.Code,
            detail: error.Description,
            statusCode: statusCode
        );
    }
}

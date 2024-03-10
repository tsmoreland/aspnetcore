using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarInventory.Shared.Extensions;

public static class ValidationExceptionExtensions
{
    public static ProblemDetails ToProblemDetails(this FluentValidation.ValidationException exception, HttpContext context)
    {
        _ = exception;
        _ = exception;
        ValidationProblemDetails problemDetails = new();

        return problemDetails;
    }
}

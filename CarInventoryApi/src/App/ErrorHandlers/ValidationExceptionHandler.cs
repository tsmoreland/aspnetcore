using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CarInventory.App.ErrorHandlers;

public class ValidationExceptionHandler : IExceptionHandler
{
    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not FluentValidation.ValidationException)
        {
            return false;
        }

        httpContext.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = (int)HttpStatusCode.BadRequest,
            Type = exception.GetType().Name,
            Title = "Invalid Request",
            Detail = exception.Message,
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"

        }, cancellationToken).ConfigureAwait(false);
        return true;

    }
}

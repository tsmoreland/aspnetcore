using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CarInventory.App.ErrorHandlers;

public sealed class BadHttpRequestExceptionHandler : IExceptionHandler
{
    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not BadHttpRequestException)
        {
            return false;
        }

        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = (int)HttpStatusCode.BadRequest,
            Type = exception.GetType().Name,
            Title = "Invalid Request",
            Detail = "One or more problems occurred with the request, please check parameters and try again",
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"

        }, cancellationToken);
        return true;
    }
}

using System.Globalization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.RateLimiting;

namespace CarInventory.Api.RateLimitPolicies;

public sealed class RateLimitPolicy : IRateLimiterPolicy<string>
{
    /// <inheritdoc />
    public RateLimitPartition<string> GetPartition(HttpContext httpContext)
    {
        return RateLimitPartition.GetFixedWindowLimiter(string.Empty, _ =>
            new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromSeconds(10),
                QueueLimit = 0,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                AutoReplenishment = true,
            });
    }

    /// <inheritdoc />
    public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected { get; } = OnRejectedHandler;

    internal static async ValueTask OnRejectedHandler(OnRejectedContext context, CancellationToken cancellationToken)
    {
        ProblemDetailsFactory factory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

        ProblemDetails problem = factory.CreateProblemDetails(
            context.HttpContext,
            StatusCodes.Status429TooManyRequests,
            title: "Too many requests",
            type: "https://www.rfc-editor.org/rfc/rfc6585#section-4",
            detail: "Unable to process requst at this time due to volume of requests",
            instance: context.HttpContext.Request.Path);

        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out TimeSpan retryAfter))
        {
            context.HttpContext.Response.Headers.RetryAfter =
                ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
        }

        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        context.HttpContext.RequestServices.GetService<ILoggerFactory>()?
            .CreateLogger("Microsoft.AspNetCore.RateLimitingMiddleware")
            .LogWarning("OnRejected: {GetUserEndPoint}", GetUserEndPoint(context.HttpContext));

        return;

        static string GetUserEndPoint(HttpContext context) =>
           $"User {context.User.Identity?.Name ?? "Anonymous"} endpoint:{context.Request.Path}"
           + $" {context.Connection.RemoteIpAddress}";
    }

}

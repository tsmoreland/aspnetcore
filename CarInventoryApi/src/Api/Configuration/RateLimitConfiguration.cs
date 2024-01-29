using CarInventory.Api.RateLimitPolicies;
using Microsoft.AspNetCore.RateLimiting;

namespace CarInventory.Api.Configuration;

public static class RateLimitConfiguration 
{
    public static void Configure(RateLimiterOptions options)
    {
        options.OnRejected = RateLimitPolicy.OnRejectedHandler;
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

        options.AddPolicy<string, RateLimitPolicy>("api");
    }
}

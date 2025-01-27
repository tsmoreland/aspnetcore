using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GloboTicket.FrontEnd.Mvc.App.HealthChecks;

/// <summary>
/// Verifies dependencies are healthy, attempting 3 times
/// </summary>
public sealed class SlowDependencyHealthCheck : IHealthCheck
{
    public static readonly string MaxHealthCheckName = "slow_dependency";
    private static int _sInvocationCount;

    /// <inheritdoc />
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        if (_sInvocationCount++ > 3)
        {
            return Task.FromResult(HealthCheckResult.Healthy("Dependency is ready"));
        }

        return Task.FromResult(new HealthCheckResult(
            status: context.Registration.FailureStatus,
            description: $"Dpendency is still initialising.  Invocation count {_sInvocationCount}"));
    }
}

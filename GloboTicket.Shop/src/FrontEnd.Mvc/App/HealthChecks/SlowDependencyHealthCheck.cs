using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GloboTicket.FrontEnd.Mvc.App.HealthChecks;

/// <summary/>
public sealed class SlowDependencyHealthCheck : IHealthCheck
{
    public static readonly string HealthCheckName = "slow_dependency";
    private static int s_invocationCount = 0;

    /// <summary/>
    public SlowDependencyHealthCheck()
    {
    }

    /// <inheritdoc />
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        if (s_invocationCount++ > 3)
        {
            return Task.FromResult(HealthCheckResult.Healthy($"Dependency is ready"));
        }

        return Task.FromResult(new HealthCheckResult(
            status: context.Registration.FailureStatus,
            description: $"Dpendency is still initialising.  Invocation count {s_invocationCount}"));
    }
}

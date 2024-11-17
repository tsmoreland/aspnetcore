using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CarInventory.Cars.Infrastructure.Persistence;

public sealed class DatabaseHealthCheck(IDbContextFactory<CarsDbContext> dbContextFactory) : IHealthCheck
{
    private readonly IDbContextFactory<CarsDbContext> _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));

    /// <inheritdoc />
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var dbContextTask = _dbContextFactory.CreateDbContextAsync(cancellationToken);
        CarsDbContext? dbContext = null;
        var isDbConnectable = false;
        // avoiding use of await using so we can use ConfigureAwait(false)
        try
        {
            dbContext = await dbContextTask.ConfigureAwait(false);
            isDbConnectable = await dbContext.Database.CanConnectAsync(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            if (dbContext is not null) await dbContext.DisposeAsync().ConfigureAwait(false);
        }

        return isDbConnectable
            ? HealthCheckResult.Healthy()
            : HealthCheckResult.Unhealthy();
    }
}

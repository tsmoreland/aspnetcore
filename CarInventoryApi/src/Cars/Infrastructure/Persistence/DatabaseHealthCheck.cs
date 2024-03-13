using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CarInventory.Cars.Infrastructure.Persistence;

public sealed class DatabaseHealthCheck(IDbContextFactory<CarsDbContext> dbContextFactory) : IHealthCheck
{
    private readonly IDbContextFactory<CarsDbContext> _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));

    /// <inheritdoc />
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        await using CarsDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        bool isDbConnectable = await dbContext.Database.CanConnectAsync(cancellationToken);

        return isDbConnectable
            ? HealthCheckResult.Healthy()
            : HealthCheckResult.Unhealthy();
    }
}

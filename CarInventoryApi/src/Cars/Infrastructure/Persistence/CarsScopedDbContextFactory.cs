using Microsoft.EntityFrameworkCore;

namespace CarInventory.Cars.Infrastructure.Persistence;

public sealed class CarsScopedDbContextFactory(IDbContextFactory<CarsDbContext> pooledFactory) : IDbContextFactory<CarsDbContext>
{
    private const int DefaultTenantId = -1;

    private readonly IDbContextFactory<CarsDbContext> _pooledFactory = pooledFactory ?? throw new ArgumentNullException(nameof(pooledFactory));
#pragma warning disable IDE0052
#pragma warning disable IDE0051
#pragma warning disable CS0414 // Field is assigned but its value is never used
    private readonly int _tenantId = DefaultTenantId; // unused at present but if in azure this would be retrieved via ITenant
#pragma warning restore CS0414 // Field is assigned but its value is never used
#pragma warning restore IDE0051
#pragma warning restore IDE0052

    /// <inheritdoc />
    public CarsDbContext CreateDbContext()
    {
        CarsDbContext context = _pooledFactory.CreateDbContext();
        // context.TentantId = _tenantId; // if/when we have tenantId
        return context;
    }

}

using CarInventory.Cars.Domain.Models;
using CarInventory.Cars.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CarInventory.Cars.Infrastructure.Persistence;

public sealed class CarsDbContext(DbContextOptions<CarsDbContext> options) : DbContext(options)
{
    public DbSet<Car> Cars => Set<Car>();

    public bool IsCarTracked(Guid id)
    {
        EntityEntry? entry = FindEntityEntry<Car>(e => e.Id == id);
        return entry is not null && entry.State != EntityState.Detached;
    }

    private EntityEntry? FindEntityEntry<TEntity>(Func<TEntity, bool> predicate)
    {
        return ChangeTracker.Entries()
            .FirstOrDefault(e => e.Entity is TEntity entity && predicate(entity));
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CarsDbContext).Assembly);
    }

    /// <inheritdoc />
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<decimal>()
            .HaveConversion<DecimalToDoubleValueConverter>();
    }
}

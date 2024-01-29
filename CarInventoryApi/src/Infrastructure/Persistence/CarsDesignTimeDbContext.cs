using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CarInventory.Infrastructure.Persistence;

public sealed class CarsDesignTimeDbContext : IDesignTimeDbContextFactory<CarsDbContext>
{
    /// <inheritdoc />
    public CarsDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<CarsDbContext> optionsBuilder = new();
        optionsBuilder.UseSqlite(@"Data=designTime.db;Pooling=false", o => o.MigrationsAssembly(typeof(CarsDbContext).Assembly.FullName));
        return new CarsDbContext(optionsBuilder.Options);
    }
}

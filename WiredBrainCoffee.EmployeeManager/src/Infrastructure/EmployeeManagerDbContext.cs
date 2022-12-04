
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WiredBrainCoffee.EmployeeManager.Infrastructure.Contracts;
using WiredBrainCoffee.EmployeeManager.Infrastructure.Converters;
using WiredBrainCoffee.EmployeeManager.Infrastructure.Entities;

namespace WiredBrainCoffee.EmployeeManager.Infrastructure;

public sealed class EmployeeManagerDbContext : DbContext
{
    private readonly IModelConfiguration _modelConfiguration;

    /// <inheritdoc />
    public EmployeeManagerDbContext(DbContextOptions options, IModelConfiguration modelConfiguration)
        : base(options)
    {
        _modelConfiguration = modelConfiguration;
    }

    public DbSet<EmployeeEntity> Employees => Set<EmployeeEntity>();
    public DbSet<DepartmentEntity> Departments => Set<DepartmentEntity>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmployeeEntity).Assembly);
        _modelConfiguration.ConfigureModel(modelBuilder);
    }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _modelConfiguration.ConfigureContext(optionsBuilder);
        base.OnConfiguring(optionsBuilder);
    }

    /// <inheritdoc />
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateTimeOffset?>()
            .HaveConversion<NullableDateTimeOffsetValueConverter>();
        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffsetValueConverter>();
    }

    /// <inheritdoc />
    public override int SaveChanges()
    {
        _modelConfiguration.SaveChangesVisitor(this);
        return base.SaveChanges();
    }

    /// <inheritdoc />
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        _modelConfiguration.SaveChangesVisitor(this);
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}

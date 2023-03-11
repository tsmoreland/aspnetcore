using CarvedRock.Domain.Entities;
using CarvedRock.Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CarvedRock.Infrastructure;

public sealed class CarvedRockDbContext : DbContext
{
    private readonly IModelConfiguration<ModelBuilder, DbContextOptionsBuilder> _configuration;

    /// <inheritdoc />
    public CarvedRockDbContext(DbContextOptions options, IModelConfiguration<ModelBuilder, DbContextOptionsBuilder> configuration)
        : base(options)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Item> Items => Set<Item>();

    public DbSet<Dictionary<string, object?>> Tags => Set<Dictionary<string, object?>>("Tag");

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _configuration.ConfigureModel(modelBuilder);
    }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _configuration.ConfigureContext(optionsBuilder);
    }
}

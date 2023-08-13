using BethanysPieShop.Admin.Domain.Models;
using BethanysPieShop.Admin.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.Admin.Infrastructure.Persistence;

public sealed class AdminDbContext : DbContext
{
    /// <inheritdoc />
    public AdminDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Pie> Pies => Set<Pie>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Order> Orders => Set<Order>();


    internal ISqlConfiguration SqlConfiguration { get; init; } = new SqlServerConfiguration();

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SqlConfiguration.ConfigureModel(modelBuilder);
    }

    /// <inheritdoc/>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        SqlConfiguration.ConfigureConventions(configurationBuilder);
    }
}

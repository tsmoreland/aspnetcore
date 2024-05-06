using Microshop.Services.Rewards.RestApi.App.Models;
using Microsoft.EntityFrameworkCore;

namespace Microshop.Services.Rewards.RestApi.App.Infrastructure.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Reward> Rewards => Set<Reward>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.Admin.Infrastructure.Persistence;

public sealed class AdminDbContext : DbContext
{
    /// <inheritdoc />
    public AdminDbContext(DbContextOptions options) : base(options)
    {
    }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}

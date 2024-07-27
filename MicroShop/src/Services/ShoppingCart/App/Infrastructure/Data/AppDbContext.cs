using MicroShop.Services.ShoppingCart.App.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.ShoppingCart.App.Infrastructure.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<CartDetails> CartDetails => Set<CartDetails>();
    public DbSet<CartHeader> CartHeaders => Set<CartHeader>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

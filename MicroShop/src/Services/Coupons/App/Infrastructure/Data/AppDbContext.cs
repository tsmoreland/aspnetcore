using MicroShop.Services.Coupons.App.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Coupons.App.Infrastructure.Data;

/// <inheritdoc />
public sealed class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Coupon> Coupons => Set<Coupon>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<Coupon>().HasData(
            new Coupon(1, "10OFF", 10.0, 20),
            new Coupon(2, "20OFF", 20.0, 40),
            new Coupon(3, "25OFF", 25.0, 50),
            new Coupon(4, "50OFF", 50.0, 100));
    }
}

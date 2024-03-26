using MicroShop.Services.Products.ProductsApiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Products.ProductsApiApp.Infrastructure.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<Product>().HasData(
            new Product(1, "Lenovo IdeaPad Slim 5", 549.99, "Travel light and stay rugged without sacrificing an inch of speed and power with the smart, slim, lightweight, and military-grade durable IdeaPad Slim 5", "Electronics", "https://m.media-amazon.com/images/W/AVIF_800250-T3/images/I/71j4CpWJMHL._AC_SX425_.jpg"),
            new Product(2, "Apple 2023 MacBook Pro laptop M3 Pro", 1899.97, "SUPERCHARGED BY M3 PRO OR M3 MAX — The Apple M3 Pro chip, with an up to 12-core CPU and up to 18-core GPU", "Electronics", "https://m.media-amazon.com/images/W/AVIF_800250-T3/images/I/61RnQM+JDTL._AC_SX679_.jpg"),
            new Product(3, "Alienware M16 Gaming Laptop - 16-inch", 2729, "16-inch high-performance gaming laptop with the latest CPU and GPU offerings", "Electronics", "https://m.media-amazon.com/images/W/AVIF_800250-T3/images/I/81FV-rLoN8L._AC_SX425_.jpg"));
    }
}

﻿using MicroShop.Services.Orders.ApiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Orders.ApiApp.Infrastructure.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<OrderHeader> OrderHeaders => Set<OrderHeader>();
    public DbSet<OrderDetails> OrderDetails => Set<OrderDetails>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

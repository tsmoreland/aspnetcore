﻿using MicroShop.Services.Email.App.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Email.App.Infrastructure.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<EmailLogEntry> EmailLogs => Set<EmailLogEntry>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

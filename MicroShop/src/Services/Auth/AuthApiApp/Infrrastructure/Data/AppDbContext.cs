﻿using MicroShop.Services.Auth.AuthApiApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Auth.AuthApiApp.Infrrastructure.Data;

public sealed class AppDbContext(DbContextOptions options) : IdentityDbContext<AppUser, AppRole, string>(options)
{
    public DbSet<AppUser> ApplicationUsers => Set<AppUser>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>().Property(e => e.Name).HasMaxLength(200).IsRequired();
    }
}


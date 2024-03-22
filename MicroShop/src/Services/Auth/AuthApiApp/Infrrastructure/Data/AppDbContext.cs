using MicroShop.Services.Auth.AuthApiApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Auth.AuthApiApp.Infrrastructure.Data;

public sealed class AppDbContext(DbContextOptions options) : IdentityDbContext<AppUser, AppRole, string>(options)
{
    public DbSet<AppUser> ApplicationUsers => Set<AppUser>();
}

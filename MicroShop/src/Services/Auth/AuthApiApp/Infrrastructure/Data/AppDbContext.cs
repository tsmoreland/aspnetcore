using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Auth.AuthApiApp.Infrrastructure.Data;

public sealed class AppDbContext(DbContextOptions options) : IdentityDbContext<IdentityUser>(options)
{

}

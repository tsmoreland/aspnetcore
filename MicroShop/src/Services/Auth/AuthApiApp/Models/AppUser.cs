using Microsoft.AspNetCore.Identity;

namespace MicroShop.Services.Auth.AuthApiApp.Models;

public sealed class AppUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;
}

using Microsoft.AspNetCore.Identity;

namespace MicroShop.Services.Auth.App.Models;

public sealed class AppUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;
}

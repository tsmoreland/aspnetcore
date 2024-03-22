using MicroShop.Services.Auth.AuthApiApp.Models;

namespace MicroShop.Services.Auth.AuthApiApp.Services.Contracts;

public interface IJwtTokenGenerator
{
    string GenerateToken(AppUser user);
}

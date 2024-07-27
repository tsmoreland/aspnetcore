using MicroShop.Services.Auth.App.Models;

namespace MicroShop.Services.Auth.App.Services.Contracts;

public interface IJwtTokenGenerator
{
    string GenerateToken(AppUser user, IEnumerable<string> roles);
}

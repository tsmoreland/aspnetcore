﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MicroShop.Services.Auth.App.Models;
using MicroShop.Services.Auth.App.Services.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MicroShop.Services.Auth.App.Services;

public sealed class JwtTokenGenerator(IOptions<ApiJwtOptions> options) : IJwtTokenGenerator
{
    private readonly ApiJwtOptions _options = options.Value;

    /// <inheritdoc />
    public string GenerateToken(AppUser user, IEnumerable<string> roles)
    {
        JwtSecurityTokenHandler handler = new();
        byte[] secret = Encoding.UTF8.GetBytes(_options.Secret);
        List<Claim> claims =
        [
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
        ];
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Audience = _options.Audience,
            Issuer = _options.Issuer,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256),
        };

        JwtSecurityToken token = handler.CreateJwtSecurityToken(tokenDescriptor);
        return handler.WriteToken(token);
    }
}

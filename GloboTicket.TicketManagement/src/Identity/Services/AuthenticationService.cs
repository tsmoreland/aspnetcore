using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GloboTicket.TicketManagement.Application.Contracts.Exceptions;
using GloboTicket.TicketManagement.Application.Contracts.Identity;
using GloboTicket.TicketManagement.Application.Models.Authentication;
using GloboTicket.TicketManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using IdentityError = GloboTicket.TicketManagement.Application.Models.Authentication.IdentityError;

namespace GloboTicket.TicketManagement.Identity.Services;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JwtSettings _jwtSettings;

    public AuthenticationService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettings, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        ArgumentNullException.ThrowIfNull(jwtSettings);
        _jwtSettings = jwtSettings.Value;
    }

    /// <inheritdoc />
    public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new IdentityException(IdentityError.UserNotFound, "User not found");
        }

        if (!user.IsValid())
        {
            throw new IdentityException(IdentityError.InvalidUser);
        }

        SignInResult result = await _signInManager.PasswordSignInAsync(user, request.Password, false, lockoutOnFailure: false); // lock out should probably be true, but after a limited number of attempts
        if (!result.Succeeded)
        {
            throw new IdentityException(IdentityError.InvalidCredentials, "User provided invalid credentials");
        }

        JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

        AuthenticationResponse response = new(user.Id, user.Email, user.UserName, new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));

        return response;
    }

    /// <inheritdoc />
    public async Task<RegistrationResponse> RegisterAsync(RegistrationRequest request)
    {
        ApplicationUser? existingUser = await _userManager.FindByNameAsync(request.UserName);

        if (existingUser is null)
        {
            throw new IdentityException(IdentityError.UserAlreadyExists, "Account already exists");
        }

        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            EmailConfirmed = true
        };

        ApplicationUser? existingEmail = await _userManager.FindByEmailAsync(request.Email);

        if (existingEmail is not null)
        {
            throw new IdentityException(IdentityError.UserAlreadyExists, "Account with provided e-mail already exists");
        }

        IdentityResult result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            return new RegistrationResponse(user.Id);
        }

        throw new IdentityException(IdentityError.ValidationError, result.Errors.ToString());
    }

    private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
    {
        if (!user.IsValid())
        {
            throw new IdentityException(IdentityError.InvalidUser);
        }

        IList<Claim> userClaims = await _userManager.GetClaimsAsync(user);
        IList<string> roles = await _userManager.GetRolesAsync(user);

        List<Claim> roleClaims = roles.Select(role => new Claim("roles", role)).ToList();

        IEnumerable<Claim> claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id.ToString())
        }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }
}

using System.Runtime.InteropServices;
using MicroShop.Services.Auth.AuthApiApp.Infrrastructure.Data;
using MicroShop.Services.Auth.AuthApiApp.Models;
using MicroShop.Services.Auth.AuthApiApp.Models.DataTransferObjects;
using MicroShop.Services.Auth.AuthApiApp.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Auth.AuthApiApp.Services;

public sealed class AuthService(AppDbContext dbContext, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IJwtTokenGenerator tokenGenerator) : IAuthService
{
    private readonly RoleManager<AppRole> _roleManager = roleManager;

    /// <inheritdoc />
    public async Task<ResponseDto<UserDto>> Register(string email, string name, string phoneNumber, string password)
    {
        AppUser user = new()
        {
            UserName = email,
            Email = email,
            NormalizedEmail = email.ToUpperInvariant(),
            Name = name,
            PhoneNumber = phoneNumber,
        };

        try
        {
            IdentityResult result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return ResponseDto.Error<UserDto>(result.Errors
                    .Select(err => err.ToString())
                    .Aggregate(string.Empty, (a, b) => $"{a}; {b}"));
            }

            AppUser userToReturn = await dbContext.ApplicationUsers.AsNoTracking().FirstAsync(e => e.UserName == email);
            return ResponseDto.Ok(new UserDto(userToReturn));

        }
        catch (Exception ex)
        {
            return ResponseDto.Error<UserDto>(ex.Message);
        }
    }

    /// <inheritdoc />
    public async Task<ResponseDto<LoginResponseDto>> Login(string username, string password)
    {
        string normalizedUsername = username.ToUpperInvariant();
        AppUser? user = await dbContext.ApplicationUsers.FirstOrDefaultAsync(e => e.NormalizedUserName == normalizedUsername);
        if (user is null)
        {
            return ResponseDto.Error<LoginResponseDto>("user not found");
        }

        IList<string> roles = await userManager.GetRolesAsync(user);

        string token = tokenGenerator.GenerateToken(user, roles);
        bool isValid = await userManager.CheckPasswordAsync(user, password);
        if (!isValid)
        {
            return ResponseDto.Error<LoginResponseDto>("invalid password");
        }

        UserDto userDto = new(user);
        return ResponseDto.Ok(new LoginResponseDto(userDto, token));
    }

    /// <inheritdoc />
    public async Task<bool> AssignRole(string email, string roleName)
    {
        string normalizedEmail = email.ToUpperInvariant();
        AppUser? user = await dbContext.ApplicationUsers.FirstOrDefaultAsync(e => e.NormalizedEmail == normalizedEmail);
        if (user is null)
        {
            return false;
        }

        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new AppRole { Name = roleName, NormalizedName = roleName.ToUpperInvariant() });
        }
        await userManager.AddToRoleAsync(user, roleName);
        return false;
    }
}

using MicroShop.Web.MvcApp.Models;
using MicroShop.Web.MvcApp.Models.Auth;
using MicroShop.Web.MvcApp.Services.Contracts;

namespace MicroShop.Web.MvcApp.Services;

public sealed class AuthService(IBaseService baseService) : IAuthService
{
    private const string ClientName = "AuthApi";

    /// <inheritdoc />
    public Task<ResponseDto<UserDto>?> Register(RegistrationRequestDto request)
    {
        return SendAsync<UserDto>(new RequestDto(ApiType.Post, "/api/auth/register", request, string.Empty));
    }

    /// <inheritdoc />
    public Task<ResponseDto<LoginResponseDto>?> Login(LoginRequestDto request)
    {
        return SendAsync<LoginResponseDto>(new RequestDto(ApiType.Post, "/api/auth/login", request, string.Empty));
    }

    /// <inheritdoc />
    public Task<ResponseDto?> AssignRole(ChangeRoleDto request)
    {
        return SendAsync(new RequestDto(ApiType.Post, "/api/auth/role", request, string.Empty));
    }

    private Task<ResponseDto<T>?> SendAsync<T>(RequestDto data)
    {
        return baseService.SendAsync<T>(ClientName, data);
    }

    private Task<ResponseDto?> SendAsync(RequestDto data)
    {
        return baseService.SendAsync(ClientName, data);
    }
}

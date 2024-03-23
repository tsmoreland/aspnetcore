using MicroShop.Web.App.Models;
using MicroShop.Web.App.Models.Auth;
using Microsoft.AspNetCore.Identity.Data;

namespace MicroShop.Web.App.Services.Contracts;

public interface IAuthService
{
    Task<ResponseDto<UserDto>?> Register(RegistrationRequestDto request);
    Task<ResponseDto<LoginResponseDto>?> Login(LoginRequestDto request);
    Task<ResponseDto?> AssignRole(ChangeRoleDto request);
}

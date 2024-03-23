using MicroShop.Web.MvcApp.Models;
using MicroShop.Web.MvcApp.Models.Auth;

namespace MicroShop.Web.MvcApp.Services.Contracts;

public interface IAuthService
{
    Task<ResponseDto<UserDto>?> Register(RegistrationRequestDto request);
    Task<ResponseDto<LoginResponseDto>?> Login(LoginRequestDto request);
    Task<ResponseDto?> AssignRole(ChangeRoleDto request);
}

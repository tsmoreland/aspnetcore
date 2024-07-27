using MicroShop.Web.Mvc.App.Models;
using MicroShop.Web.Mvc.App.Models.Auth;

namespace MicroShop.Web.Mvc.App.Services.Contracts;

public interface IAuthService
{
    Task<ResponseDto<UserDto>?> Register(RegistrationRequestDto request);
    Task<ResponseDto<LoginResponseDto>?> Login(LoginRequestDto request);
    Task<ResponseDto?> AssignRole(ChangeRoleDto request);
    Task<ResponseDto<IEnumerable<string>>?> GetRoles();
}

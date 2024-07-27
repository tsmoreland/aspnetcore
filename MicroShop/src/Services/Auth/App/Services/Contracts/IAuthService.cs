using MicroShop.Services.Auth.App.Models.DataTransferObjects;

namespace MicroShop.Services.Auth.App.Services.Contracts;

public interface IAuthService
{
    Task<ResponseDto<UserDto>> Register(string email, string name, string phoneNumber, string password);
    Task<ResponseDto<LoginResponseDto>> Login(string username, string password);
    Task<bool> AssignRole(string email, string roleName);
}

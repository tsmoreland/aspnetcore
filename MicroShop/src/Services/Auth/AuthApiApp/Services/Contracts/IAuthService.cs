using MicroShop.Services.Auth.AuthApiApp.Models.DataTransferObjects;

namespace MicroShop.Services.Auth.AuthApiApp.Services.Contracts;

public interface IAuthService
{
    Task<ResponseDto<UserDto>> Register(string email, string name, string phoneNumber, string password);
    Task<ResponseDto<LoginResponseDto>> Login(string username, string password);
    Task<bool> AssignRole(string email, string roleName);
}

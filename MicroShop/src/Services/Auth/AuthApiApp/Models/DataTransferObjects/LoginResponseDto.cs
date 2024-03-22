namespace MicroShop.Services.Auth.AuthApiApp.Models.DataTransferObjects;

public sealed record class LoginResponseDto(UserDto User, string Token);

namespace MicroShop.Services.Auth.App.Models.DataTransferObjects;

public sealed record class LoginResponseDto(UserDto User, string Token);

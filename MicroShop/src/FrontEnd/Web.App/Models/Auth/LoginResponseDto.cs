namespace MicroShop.Web.App.Models.Auth;

public sealed record class LoginResponseDto(UserDto User, string Token);

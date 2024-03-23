namespace MicroShop.Web.MvcApp.Models.Auth;

public sealed record class LoginResponseDto(UserDto User, string Token);

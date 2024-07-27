namespace MicroShop.Web.Mvc.App.Models.Auth;

public sealed record class LoginResponseDto(UserDto User, string Token);

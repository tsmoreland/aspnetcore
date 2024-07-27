namespace MicroShop.Services.Auth.App.Models.DataTransferObjects;

public sealed record class LoginRequestDto(string UserName, string Password);

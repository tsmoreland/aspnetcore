namespace MicroShop.Services.Auth.AuthApiApp.Models.DataTransferObjects;

public sealed record class LoginRequestDto(string UserName, string Password);

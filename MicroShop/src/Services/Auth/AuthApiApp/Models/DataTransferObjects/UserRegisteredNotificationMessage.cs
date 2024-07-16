namespace MicroShop.Services.Auth.AuthApiApp.Models.DataTransferObjects;

public sealed record class UserRegisteredNotificationMessage(string Name, string EmailAddress, string VerifyUrl);

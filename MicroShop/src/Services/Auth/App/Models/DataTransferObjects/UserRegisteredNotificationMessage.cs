namespace MicroShop.Services.Auth.App.Models.DataTransferObjects;

public sealed record class UserRegisteredNotificationMessage(string Name, string EmailAddress, string VerifyUrl);

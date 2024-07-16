namespace MicroShop.Services.Email.ApiApp.Models.DataTransferObjects;

public sealed record class UserRegisteredNotificationMessage(string Name, string EmailAddress, string VerifyUrl);

namespace MicroShop.Services.Email.App.Models.DataTransferObjects;

public sealed record class UserRegisteredNotificationMessage(string Name, string EmailAddress, string VerifyUrl);

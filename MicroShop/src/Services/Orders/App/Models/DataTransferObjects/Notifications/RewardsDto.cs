namespace MicroShop.Services.Orders.App.Models.DataTransferObjects.Notifications;

public sealed record class RewardsDto(string UserId, int RewardsActivity, int OrderId);

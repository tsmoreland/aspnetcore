namespace MicroShop.Services.Orders.ApiApp.Models.DataTransferObjects.Notifications;

public sealed record class RewardsDto(string UserId, int RewardsActivity, int OrderId);

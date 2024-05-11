namespace MicroShop.Services.Rewards.RestApi.App.Models.DataTransferObjects;

public sealed record class RewardsDto(string UserId, int RewardsActivity, int OrderId)
{
    public Reward ToNewModel() =>
        new(UserId, DateTime.UtcNow, RewardsActivity, OrderId);
}

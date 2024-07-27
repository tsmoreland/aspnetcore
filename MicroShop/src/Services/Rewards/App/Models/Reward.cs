namespace MicroShop.Services.Rewards.App.Models;

public sealed class Reward
{
    private Reward(int id, string userId, DateTime earnedDate, int rewardPoints, int orderId)
    {
        Id = id;
        UserId = userId;
        EarnedDate = earnedDate;
        RewardPoints = rewardPoints;
        OrderId = orderId;
    }

    public Reward(string userId, DateTime earnedDate, int rewardPoints, int orderId)
        : this(0, userId, earnedDate, rewardPoints, orderId)
    {
        // TODO: validation on paramters
    }

    public int Id { get; private init; }
    public string UserId { get; private init; }
    public DateTime EarnedDate { get; private init; }
    public int RewardPoints { get; private init; }
    public int OrderId { get; private init; }

}

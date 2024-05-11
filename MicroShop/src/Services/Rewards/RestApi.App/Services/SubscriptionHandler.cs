using System.Text.Json;
using MicroShop.Integrations.MessageConsumer.Abstractions;
using MicroShop.Services.Rewards.RestApi.App.Infrastructure.Data;
using MicroShop.Services.Rewards.RestApi.App.Models;
using MicroShop.Services.Rewards.RestApi.App.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Rewards.RestApi.App.Services;

public sealed class SubscriptionHandler(IDbContextFactory<AppDbContext> dbContextFactory, ILogger<SubscriptionHandler> logger) : ISubscriptionHandler
{
    /// <inheritdoc />
    public async ValueTask HandleMessage(TopicSubscriptionType subscriptionType, BinaryData messageBody)
    {
        if (subscriptionType is TopicSubscriptionType.RewardsUpdate)
        {
            await HandleRewardsUpdate(messageBody);
        }
        else
        {
            logger.LogWarning("Ignore subscription {SubscriptionType} as we should not be subscribed", subscriptionType);
        }
    }

    private async ValueTask HandleRewardsUpdate(BinaryData messageBody)
    {
        await using Stream bodyStream = messageBody.ToStream();
        RewardsDto? rewardDto = await JsonSerializer.DeserializeAsync<RewardsDto>(bodyStream);
        if (rewardDto is null)
        {
            logger.LogError("Unable to deserialized message body for rewards update");
            return;
        }

        logger.LogInformation("Reward update from Order {OrderId} Reward amount {RewardAmount}", rewardDto.OrderId, rewardDto.RewardsActivity);

        Reward reward = rewardDto.ToNewModel();
        await using AppDbContext dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.Rewards.Add(reward);
        await dbContext.SaveChangesAsync();
    }
}

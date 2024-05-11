using System.Text.Json;
using MicroShop.Integrations.MessageConsumer.Abstractions;
using MicroShop.Services.Email.ApiApp.Infrastructure.Data;
using MicroShop.Services.Email.ApiApp.Models;
using MicroShop.Services.Email.ApiApp.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Email.ApiApp.Services;

public sealed class SubscriptionHandler(IDbContextFactory<AppDbContext> dbContextFactory, ILogger<SubscriptionHandler> logger) : ISubscriptionHandler
{
    /// <inheritdoc />
    public async ValueTask HandleMessage(TopicSubscriptionType subscriptionType, BinaryData messageBody)
    {
        if (subscriptionType is TopicSubscriptionType.Email)
        {
            await HandleOrderCreatedMessage(messageBody);
        }
        else
        {
            logger.LogWarning("Ignore subscription {SubscriptionType} as we should not be subscribed", subscriptionType);
        }
    }

    private async ValueTask HandleOrderCreatedMessage(BinaryData messageBody)
    {
        await using Stream bodyStream = messageBody.ToStream();
        RewardsDto? rewardDto = await JsonSerializer.DeserializeAsync<RewardsDto>(bodyStream);
        if (rewardDto is null)
        {
            logger.LogError("Unable to deserialized message body for rewards update");
            return;
        }

        EmailLogEntry logEntry = rewardDto.ToLogOrderCreatedEntry(await GetEmailAddressForUser(rewardDto.UserId));
        await using AppDbContext dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.EmailLogs.Add(logEntry);
        await dbContext.SaveChangesAsync();
    }

    private ValueTask<string> GetEmailAddressForUser(string userId)
    {
        _ = userId;
        logger.LogWarning("returning hard coded e-mail for now, need to go to auth api to get email");
        // pending, we may need to reach out to auth api for it
        return ValueTask.FromResult("none@127.0.0.1");
    }
}

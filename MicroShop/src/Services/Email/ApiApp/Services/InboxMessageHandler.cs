using System.Text.Json;
using MicroShop.Integrations.MessageConsumer.Abstractions;
using MicroShop.Services.Email.ApiApp.Infrastructure.Data;
using MicroShop.Services.Email.ApiApp.Models;
using MicroShop.Services.Email.ApiApp.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Email.ApiApp.Services;

public sealed class InboxMessageHandler(IDbContextFactory<AppDbContext> dbContextFactory, ILogger<InboxMessageHandler> logger) : IMessageHandler
{
    /// <inheritdoc />
    public async ValueTask HandleMessage(BinaryData messageBody) // TODO: Update to take CancellationToken
    {
        await using Stream bodyStream = messageBody.ToStream();
        EmailMessage email = await JsonSerializer.DeserializeAsync<EmailMessage>(bodyStream) ?? throw new JsonException("Unable to deserialize message content");

        logger.LogInformation("Email Received {Name} {Email} {CartTotal}", email.Name, email.EmailAddress, email.Content.CartTotal);

        EmailLogEntry logEntry = email.ToLogEntry();
        await using AppDbContext dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.EmailLogs.Add(logEntry);
        await dbContext.SaveChangesAsync();
    }
}

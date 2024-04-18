using System.Text.Json;
using MicroShop.Integrations.MessageConsumer.Abstractions;
using MicroShop.Services.Email.ApiApp.Infrastructure.Data;
using MicroShop.Services.Email.ApiApp.Models;
using MicroShop.Services.Email.ApiApp.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Email.ApiApp.Services;

public sealed class MessageHandler(IDbContextFactory<AppDbContext> dbContextFactory, ILogger<MessageHandler> logger) : IMessageHandler
{
    /// <inheritdoc />
    public async ValueTask HandleMessage(MessageType messageType, BinaryData messageBody) // TODO: Update to take CancellationToken
    {
        switch (messageType)
        {
            case MessageType.ShoppingCart:
                await HandleShoppingCartMessage(messageBody);
                break;
            case MessageType.RegisterUser:
                await HandleRegisterUserMessage(messageBody);
                break;
            case MessageType.None:
            default:
                break; // should be logged by caller - we shouldn't get here for anything but the first two
        }
    }

    public async ValueTask HandleShoppingCartMessage(BinaryData messageBody)
    {
        await using Stream bodyStream = messageBody.ToStream();
        EmailMessage email = await JsonSerializer.DeserializeAsync<EmailMessage>(bodyStream) ?? throw new JsonException("Unable to deserialize message content");

        logger.LogInformation("Email Received {Name} {Email} {CartTotal}", email.Name, email.EmailAddress, email.Content.CartTotal);

        EmailLogEntry logEntry = email.ToLogEntry();
        await using AppDbContext dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.EmailLogs.Add(logEntry);
        await dbContext.SaveChangesAsync();
    }

    public async ValueTask HandleRegisterUserMessage(BinaryData messageBody)
    {
        await using Stream bodyStream = messageBody.ToStream();
        string emailAddress = await JsonSerializer.DeserializeAsync<string>(bodyStream) ?? throw new JsonException("Unable to deserialize message content");

        logger.LogInformation("Email Received {Email}", emailAddress);

        EmailLogEntry logEntry = new(emailAddress, $"User registration successful<br/> Email: {emailAddress} ", DateTime.UtcNow);
        await using AppDbContext dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.EmailLogs.Add(logEntry);
        await dbContext.SaveChangesAsync();
    }
}

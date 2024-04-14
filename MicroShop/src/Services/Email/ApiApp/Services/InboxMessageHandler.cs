using System.Text.Json;
using MicroShop.Integrations.MessageConsumer.Abstractions;
using MicroShop.Services.Email.ApiApp.Models.DataTransferObjects;

namespace MicroShop.Services.Email.ApiApp.Services;

public sealed class InboxMessageHandler(ILogger<InboxMessageHandler> logger) : IMessageHandler
{
    /// <inheritdoc />
    public async ValueTask HandleMessage(BinaryData messageBody)
    {
        await using Stream bodyStream = messageBody.ToStream();
        EmailMessage email = await JsonSerializer.DeserializeAsync<EmailMessage>(bodyStream) ?? throw new JsonException("Unable to deserialize message content");

        logger.LogInformation("Email Received {Name} {Email} {CartTotal}", email.Name, email.EmailAddress, email.Content.CartTotal);
    }
}

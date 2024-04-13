using System.Text;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;

namespace MicroShop.Integrations.MessageBus.AzureMessageBus;

public sealed class MessageBus(IOptions<MessageBusOptions> options) : IMessageBus
{
    private readonly string _connectionString = options.Value.ConnectionString;

    /// <inheritdoc />
    public async ValueTask PublishMessage<T>(string name, T content)
    {
        if (name is not { Length: > 0 })
        {
            return;
        }

        await using ServiceBusClient client = new(_connectionString);
        await using ServiceBusSender sender = client.CreateSender(name);

        byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(content));
        ServiceBusMessage message = new(data)
        {
            CorrelationId = Guid.NewGuid().ToString(),
        };

        await sender.SendMessageAsync(message);
    }
}

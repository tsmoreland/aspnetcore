using System.Runtime.CompilerServices;
using Azure.Messaging.ServiceBus;
using MicroShop.Integrations.MessageConsumer.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MicroShop.Integrations.MessageConsumer.AzureMessageConsumer;

public sealed class MessageConsumer(IMessageHandler handler, IOptions<MessageConsumerOptions> options, IOptions<AzureMessageBusOptions> azureOptions, ILogger<MessageConsumer> logger)
    : ConsumerBase<MessageType>(azureOptions, logger)
{
    /// <inheritdoc />
    [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
    protected override bool MessageTypeIsNone(MessageType messageType) => messageType is MessageType.None;

    /// <inheritdoc />
    protected override IEnumerable<(MessageType Type, string Name)> GetTypesAndNamesFromOptions() =>
        options.Value.Queues.Select(static q => (q.Type, q.Name));

    /// <inheritdoc />
    protected override void RegisterEvents(ServiceBusProcessor processor, MessageType type)
    {
        switch (type)
        {
            case MessageType.ShoppingCart:
                processor.ProcessMessageAsync += OnCartMessageRecieved;
                break;
            case MessageType.RegisterUser:
                processor.ProcessMessageAsync += OnRegisterMessageRecieved;
                break;
            case MessageType.None:
            default:
                Logger.LogWarning("Unrecognized or unsupported message type {MessageType}", type);
                break;
        }
    }

    /// <inheritdoc />
    protected override void UnregisterEvents(ServiceBusProcessor processor, MessageType type)
    {
        switch (type)
        {
            case MessageType.ShoppingCart:
                processor.ProcessMessageAsync -= OnCartMessageRecieved;
                break;
            case MessageType.RegisterUser:
                processor.ProcessMessageAsync -= OnRegisterMessageRecieved;
                break;
            case MessageType.None:
            default:
                Logger.LogWarning("Unrecognized or unsupported message type {MessageType}", type);
                break;
        }
    }

    private async Task OnCartMessageRecieved(ProcessMessageEventArgs eventArgs)
    {
        await OnMessageRecieved(MessageType.ShoppingCart, eventArgs);
    }
    private async Task OnRegisterMessageRecieved(ProcessMessageEventArgs eventArgs)
    {
        await OnMessageRecieved(MessageType.RegisterUser, eventArgs);
    }
    private async Task OnMessageRecieved(MessageType messageType, ProcessMessageEventArgs eventArgs)
    {
        await handler.HandleMessage(messageType, eventArgs.Message.Body);
        await eventArgs.CompleteMessageAsync(eventArgs.Message);
    }
}

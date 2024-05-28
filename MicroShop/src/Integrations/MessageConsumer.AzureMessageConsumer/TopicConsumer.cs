using System.Runtime.CompilerServices;
using Azure.Messaging.ServiceBus;
using MicroShop.Integrations.MessageConsumer.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MicroShop.Integrations.MessageConsumer.AzureMessageConsumer;

public sealed class TopicConsumer(ISubscriptionHandler handler, IOptions<TopicConsumerOptions> options, IOptions<AzureMessageBusOptions> azureOptions, ILogger<TopicConsumer> logger)
    : ConsumerBase<TopicSubscriptionType>(azureOptions, logger)
{
    /// <inheritdoc />
    [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
    protected override bool MessageTypeIsNone(TopicSubscriptionType messageType) =>
        messageType is TopicSubscriptionType.None;

    /// <inheritdoc />
    protected override IEnumerable<(TopicSubscriptionType Type, string Name)> GetTypesAndNamesFromOptions() =>
        options.Value.Subscriptions.Select(static s => (s.Type, s.Name));

    /// <inheritdoc />
    protected override void RegisterEvents(ServiceBusProcessor processor, TopicSubscriptionType type)
    {
        switch (type)
        {
            case TopicSubscriptionType.RewardsUpdate:
                processor.ProcessMessageAsync += OnRewardsUpdateMessageReceived;
                break;
            case TopicSubscriptionType.Email:
                processor.ProcessMessageAsync += OnEmailMessageReceived;
                break;
            case TopicSubscriptionType.None:
            default:
                Logger.LogWarning("Unrecognized or unsupported message type {SubscriptionType}", type);
                break;
        }
    }

    /// <inheritdoc />
    protected override void UnregisterEvents(ServiceBusProcessor processor, TopicSubscriptionType type)
    {
        switch (type)
        {
            case TopicSubscriptionType.RewardsUpdate:
                processor.ProcessMessageAsync -= OnRewardsUpdateMessageReceived;
                break;
            case TopicSubscriptionType.Email:
                processor.ProcessMessageAsync -= OnEmailMessageReceived;
                break;
            case TopicSubscriptionType.None:
            default:
                Logger.LogWarning("Unrecognized or unsupported message type {SubscriptionType}", type);
                break;
        }
    }

    private Task OnEmailMessageReceived(ProcessMessageEventArgs eventArgs) => OnMessageReceived(TopicSubscriptionType.Email, eventArgs);
    private Task OnRewardsUpdateMessageReceived(ProcessMessageEventArgs eventArgs) => OnMessageReceived(TopicSubscriptionType.RewardsUpdate, eventArgs);
    private async Task OnMessageReceived(TopicSubscriptionType topicSubscriptionType, ProcessMessageEventArgs eventArgs)
    {
        await handler.HandleMessage(topicSubscriptionType, eventArgs.Message.Body);
        await eventArgs.CompleteMessageAsync(eventArgs.Message);
    }
}

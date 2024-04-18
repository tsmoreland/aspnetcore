using System.Text;
using Azure.Messaging.ServiceBus;
using MicroShop.Integrations.MessageConsumer.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MicroShop.Integrations.MessageConsumer.AzureMessageConsumer;

public sealed class MessageConsumer : IHostedService, IAsyncDisposable
{
    private readonly IMessageHandler _handler;
    private readonly ILogger<MessageConsumer> _logger;
    // could potentially use 2 maps, 1 for processor by type and another for type by processor id - that would allow a single on message and on error method
    private readonly IReadOnlyDictionary<MessageType, ServiceBusProcessor> _processors;

    public MessageConsumer(IMessageHandler handler,  IOptions<MessageConsumerOptions> options, IOptions<AzureMessageBusOptions> azureOptions, ILogger<MessageConsumer> logger)
    {
        _handler = handler;
        _logger = logger;
        AzureMessageBusOptions optionsValue = azureOptions.Value;
        ServiceBusClient client = new(optionsValue.ConnectionString);

        _processors = options.Value.Queues
            .Select(q => new { q.Type, Processor = client.CreateProcessor(q.Name) })
            .ToDictionary(pair => pair.Type, pair => pair.Processor);
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
        string content = Encoding.UTF8.GetString(eventArgs.Message.Body);
        _ = content;

        try
        {
            await _handler.HandleMessage(messageType, eventArgs.Message.Body);
            await eventArgs.CompleteMessageAsync(eventArgs.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred processing message");
        }
    }
    private Task OnErrorReceived(ProcessErrorEventArgs eventArgs)
    {
        _logger.LogError(eventArgs.Exception, "Error received from Message Bus in {ErrorSource}", eventArgs.ErrorSource);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach ((MessageType type, ServiceBusProcessor processor) in _processors)
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
                    this._logger.LogWarning("Unrecognized or unsupported message type {MessageType}", type);
                    break;
            }
            processor.ProcessErrorAsync += OnErrorReceived;
            await processor.StartProcessingAsync(cancellationToken);
        }

    }

    /// <inheritdoc />
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        foreach ((MessageType type, ServiceBusProcessor processor) in _processors)
        {
            try
            {
                await processor.StopProcessingAsync(cancellationToken);
            }
            finally
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
                        this._logger.LogWarning("Unrecognized or unsupported message type {MessageType}", type);
                        break;
                }
                processor.ProcessErrorAsync -= OnErrorReceived;
            }
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await Task.WhenAll(_processors.Values.Select(p => p.DisposeAsync().AsTask()));
    }
}

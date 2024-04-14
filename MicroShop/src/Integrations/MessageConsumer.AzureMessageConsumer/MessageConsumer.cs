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
    private readonly ServiceBusProcessor _processor;

    public MessageConsumer(IMessageHandler handler, IOptions<AzureMessageBusOptions> options, ILogger<MessageConsumer> logger)
    {
        _handler = handler;
        _logger = logger;
        AzureMessageBusOptions optionsValue = options.Value;
        ServiceBusClient client = new(optionsValue.ConnectionString);
        _processor = client.CreateProcessor(optionsValue.QueueName);

    }

    private async Task OnMessageRecieved(ProcessMessageEventArgs eventArgs)
    {
        string content = Encoding.UTF8.GetString(eventArgs.Message.Body);
        _ = content;

        try
        {
            await _handler.HandleMessage(eventArgs.Message.Body);
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
        _processor.ProcessMessageAsync += OnMessageRecieved;
        _processor.ProcessErrorAsync += OnErrorReceived;
        await _processor.StartProcessingAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _processor.StopProcessingAsync(cancellationToken);
        }
        finally
        {
            _processor.ProcessMessageAsync -= OnMessageRecieved;
            _processor.ProcessErrorAsync -= OnErrorReceived;
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync() =>
        await _processor.DisposeAsync();
}

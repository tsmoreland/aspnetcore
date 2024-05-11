using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MicroShop.Integrations.MessageConsumer.AzureMessageConsumer;

public abstract class ConsumerBase<TMessageType>(IOptions<AzureMessageBusOptions> options, ILogger logger) : IHostedService, IAsyncDisposable
    where TMessageType : System.Enum, IComparable
{
    protected ILogger Logger { get; } = logger;
    private IReadOnlyDictionary<TMessageType, ServiceBusProcessor>? _processors;


    /// <inheritdoc />
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        AzureMessageBusOptions optionsValue = options.Value;
        ServiceBusClient client = new(optionsValue.ConnectionString);

        _processors = GetTypesAndNamesFromOptions()
            .Select(pair => new { pair.Type, Processor = client.CreateProcessor(pair.Name) })
            .ToDictionary(pair => pair.Type, pair => pair.Processor);

        List<Task> tasks = [];
        foreach ((TMessageType type, ServiceBusProcessor processor) in _processors)
        {
            if (MessageTypeIsNone(type))
            {
                continue;
            }

            RegisterEvents(processor, type);
            processor.ProcessErrorAsync += OnErrorReceived;
            tasks.Add(processor.StartProcessingAsync(cancellationToken));
        }
        await Task.WhenAll([..tasks]);
    }

    private Task OnErrorReceived(ProcessErrorEventArgs eventArgs)
    {
        Logger.LogError(eventArgs.Exception, "Error received from Message Bus in {ErrorSource}", eventArgs.ErrorSource);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_processors is null)
        {
            return;
        }

        foreach ((TMessageType type, ServiceBusProcessor processor) in _processors)
        {
            try
            {
                await processor.StopProcessingAsync(cancellationToken);
            }
            finally
            {
                if (!MessageTypeIsNone(type))
                {
                    UnregisterEvents(processor, type);
                    processor.ProcessErrorAsync -= OnErrorReceived;
                }
            }
        }
    }

    protected abstract bool MessageTypeIsNone(TMessageType messageType);
    protected abstract IEnumerable<(TMessageType Type, string Name)> GetTypesAndNamesFromOptions();
    protected abstract void RegisterEvents(ServiceBusProcessor processor, TMessageType type);
    protected abstract void UnregisterEvents(ServiceBusProcessor processor, TMessageType type);

    protected virtual ValueTask DisposeAsync(bool disposing)
    {
        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);

        if (_processors is not null)
        {
            await Task.WhenAll(_processors.Values.Select(p => p.DisposeAsync().AsTask()));
        }
    }
}

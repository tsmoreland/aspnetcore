using System.Diagnostics.CodeAnalysis;
using MicroShop.Services.Email.App.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace MicroShop.Services.Email.App.Services;

public abstract class RabbitMqConsumer(
    IOptions<RabbitConnectionSettings> connectionSettings,
    IOptions<RabbitSettings> settings,
    ILogger logger) : BackgroundService
{
    private bool _connectionDisposed;
    protected ILogger Logger { get; } = logger;

    private readonly IConnection _connection =
        new ConnectionFactory
        {
            HostName = connectionSettings.Value.Hostname,
            UserName = connectionSettings.Value.Username,
            Password = connectionSettings.Value.Password,
        }.CreateConnection();

    protected bool CreateChannelForQueue(string name, [NotNullWhen(true)] out IModel? channel, [NotNullWhen(true)] out string? queueName)
    {
        if (!settings.Value.TryGetQueueByName(name, out RabbitQueue? queue))
        {
            Logger.LogError("{Name} queue not found in configuration", name);
            channel = null;
            queueName = null;
            return false;
        }

        channel = _connection.CreateModel();
        channel.QueueDeclare(queue.QueueName, false, false, false, new Dictionary<string, object>());
        queueName = queue.QueueName;
        return true;
    }

    ~RabbitMqConsumer() => Dispose(false);
    protected virtual void Dispose(bool disposing)
    {
        if (_connectionDisposed)
        {
            return;
        }
        _connectionDisposed = true;
        _connection.Dispose();

    }

    /// <inheritdoc />
    public override void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

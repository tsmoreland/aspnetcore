using System.Diagnostics.CodeAnalysis;
using MicroShop.Services.Email.App.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace MicroShop.Services.Email.App.Services;

public abstract class RabbitMqConsumer(
    IOptions<RabbitConnectionSettings> connectionSettings, ILogger logger) : BackgroundService
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

    protected bool CreateChannelForQueue(RabbitQueue queue, [NotNullWhen(true)] out IModel? channel)
    {
        channel = _connection.CreateModel();
        channel.QueueDeclare(queue.QueueName, false, false, false, new Dictionary<string, object>());
        return true;
    }

    protected bool CreateChannelForExchange(RabbitExchange exchange, [NotNullWhen(true)] out IModel? channel, [NotNullWhen(true)] out string? queueName)
    {
        channel = _connection.CreateModel();
        channel.ExchangeDeclare(exchange.ExchangeName, exchange.Type, exchange.Durable, false, new Dictionary<string, object>());
        if (exchange.QueueName is { Length: > 0 })
        {
            channel.QueueDeclare(exchange.QueueName, exchange.Durable, false, false, new Dictionary<string, object>());
            queueName = exchange.QueueName;
        }
        else
        {
            queueName = channel.QueueDeclare().QueueName; // Default name
        }
        channel.QueueBind(queueName, exchange.ExchangeName, string.Empty, new Dictionary<string, object>());

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

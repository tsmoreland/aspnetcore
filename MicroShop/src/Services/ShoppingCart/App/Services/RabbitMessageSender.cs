using System.Text.Json;
using MicroShop.Services.ShoppingCart.App.Models;
using MicroShop.Services.ShoppingCart.App.Services.Contracts;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using IRabbitChannel = RabbitMQ.Client.IModel;

namespace MicroShop.Services.ShoppingCart.App.Services;

public sealed class RabbitMessageSender(IOptions<RabbitConnectionSettings> settings) : IRabbitMessageSender, IDisposable
{
    private IRabbitChannel? _channel;
    private readonly IConnection _connection =
        new ConnectionFactory
        {
            HostName = settings.Value.Hostname,
            UserName = settings.Value.Username,
            Password = settings.Value.Password,
        }.CreateConnection();
    private bool _disposed;

    /// <inheritdoc />
    public void SendMessage<T>(T message, RabbitQueue queue)
    {
        ThrowIfDisposed();
        IRabbitChannel channel = EnsureChannelExists(queue);
        channel.BasicPublish(queue.ExchangeName, routingKey: queue.QueueName, body: JsonSerializer.SerializeToUtf8Bytes(message), basicProperties: null);
    }

    private IRabbitChannel EnsureChannelExists(RabbitQueue queue)
    {
        if (_channel is not null)
        {
            return _channel;
        }

        _channel = _connection.CreateModel();
        // TODO: move durable, exclusive, ... into RabbitQueue
        _channel.QueueDeclare(queue.QueueName, false, false, false, new Dictionary<string, object>());

        return _channel;
    }
    private void ThrowIfDisposed() =>
        ObjectDisposedException.ThrowIf(_disposed, this);

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;

        _channel = null;
        _connection.Dispose();
    }

}

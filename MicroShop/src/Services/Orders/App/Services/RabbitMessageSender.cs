using System.Text.Json;
using MicroShop.Services.Orders.App.Models;
using MicroShop.Services.Orders.App.Services.Contracts;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using IRabbitChannel = RabbitMQ.Client.IModel;

namespace MicroShop.Services.Orders.App.Services;

public sealed class RabbitMessageSender(IOptions<RabbitConnectionSettings> settings) : IRabbitMessageSender, IDisposable
{
    private bool _disposed;
    private IRabbitChannel? _channel;
    private readonly IConnection _connection =
        new ConnectionFactory
        {
            HostName = settings.Value.Hostname,
            UserName = settings.Value.Username,
            Password = settings.Value.Password,
        }.CreateConnection();

    /// <inheritdoc />
    public void SendMessage<T>(T message, RabbitExchange exchange)
    {
        ThrowIfDisposed();
        IRabbitChannel channel = EnsureChannelExists(exchange);
        channel.BasicPublish(exchange.ExchangeName, routingKey: string.Empty, basicProperties: null, JsonSerializer.SerializeToUtf8Bytes(message));
    }

    private IRabbitChannel EnsureChannelExists(RabbitExchange exchange)
    {
        if (_channel is not null)
        {
            return _channel;
        }

        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange.ExchangeName, exchange.Type, exchange.Durable);
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

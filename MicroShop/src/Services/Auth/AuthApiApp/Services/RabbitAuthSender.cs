using System.Text;
using System.Text.Json;
using MicroShop.Services.Auth.AuthApiApp.Models;
using MicroShop.Services.Auth.AuthApiApp.Services.Contracts;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace MicroShop.Services.Auth.AuthApiApp.Services;

public sealed class RabbitAuthSender(IOptions<RabbitConnectionSettings> settings) : IRabbitAuthSender, IDisposable
{
    private readonly IConnection _connection =
        new ConnectionFactory
        {
            HostName = settings.Value.Hostname,
            UserName = settings.Value.Username,
            Password = settings.Value.Password,
        }.CreateConnection();
    private bool _disposed;

    /// <inheritdoc />
    public void SendMessage<T>(T message, string queueName)
    {
        using IModel channel = _connection.CreateModel();
        channel.QueueDeclare(queueName);

        byte[] body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        channel.BasicPublish(exchange: "", routingKey: queueName, body: body);
    }

    private void Dispose(bool disposing)
    {
        if (!disposing || _disposed)
        {
            return;
        }
        _disposed = true;
        _connection.Dispose();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
    }
}

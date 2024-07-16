using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;
using MicroShop.Integrations.MessageConsumer.Abstractions;
using MicroShop.Services.Email.ApiApp.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroShop.Services.Email.ApiApp.Services;

public sealed class RabbitMqAuthConsumer(
    IOptions<RabbitConnectionSettings> connectionSettings,
    IOptions<RabbitSettings> settings,
    IMessageHandler messageHandler,
    ILogger<RabbitMqAuthConsumer> logger) : BackgroundService
{
    private bool _disposed;
    private readonly IConnection _connection =
        new ConnectionFactory
        {
            HostName = connectionSettings.Value.Hostname,
            UserName = connectionSettings.Value.Username,
            Password = connectionSettings.Value.Password,
        }.CreateConnection();

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(0, stoppingToken).ConfigureAwait(false);

        if (!settings.Value.TryGetQueueByName("RegisterUser", out RabbitQueue? queue))
        {
            logger.LogError("RegisterUser queue not found in configuration");
            return;
        }

        IModel channel = _connection.CreateModel();
        // TODO: move these additional flags to RabbitQueue class.
        channel.QueueDeclare(queue.QueueName, false, false, false, new Dictionary<string, object>());

        EventingBasicConsumer consumer = new(channel);
        consumer.Received += ConsumerReceived;

        while (!stoppingToken.IsCancellationRequested)
        {

        }
        return;

        void ConsumerReceived(object? sender, BasicDeliverEventArgs e) 
        {
            // TODO: changes this to registration dto, or an email object
            string? email = JsonSerializer.Deserialize<string>(e.Body.ToArray());
            if (email is null)
            {
                return;
            }
            // use e-mail service here to send the e-mail
        }
    }

    public override void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;

        _connection.Dispose();
        base.Dispose();
    }
}

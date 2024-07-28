using System.Text.Json;
using MicroShop.Services.Email.App.Models;
using MicroShop.Services.Email.App.Models.DataTransferObjects;
using MicroShop.Services.Email.App.Services.Contracts;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroShop.Services.Email.App.Services;

public sealed class RabbitMqOrderConsumer(
    IOptions<RabbitConnectionSettings> connectionSettings,
    IOptionsMonitor<RabbitExchange> namedExchanges,
    IEmailService emailService,
    ILogger<RabbitMqAuthConsumer> logger) : RabbitMqConsumer(connectionSettings, logger)
{
    private readonly RabbitExchange _exchange = namedExchanges.Get("OrderCreated");

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(0, stoppingToken).ConfigureAwait(false);
        if (!CreateChannelForExchange(_exchange, out IModel? channel, out string? queueName))
        {
            return;
        }

        EventingBasicConsumer consumer = new(channel);
        consumer.Received += ConsumerReceived;
        stoppingToken.ThrowIfCancellationRequested();
        channel.BasicConsume(queueName, false, consumer);

        return;

        void ConsumerReceived(object? sender, BasicDeliverEventArgs e)
        {
            try
            {
                RewardsDto? message = JsonSerializer.Deserialize<RewardsDto>(e.Body.Span);
                if (message is null)
                {
                    return;
                }
                // TODO: determine the e-mail address
                emailService.Log(message.ToLogOrderCreatedEntry("root@127.0.0.1"));
            }
            finally
            {
                channel.BasicAck(e.DeliveryTag, false);
            }
        }
    }
}

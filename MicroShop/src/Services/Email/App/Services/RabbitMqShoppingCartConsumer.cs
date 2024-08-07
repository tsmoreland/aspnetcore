﻿using System.Text.Json;
using MicroShop.Services.Email.App.Models;
using MicroShop.Services.Email.App.Models.DataTransferObjects;
using MicroShop.Services.Email.App.Services.Contracts;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroShop.Services.Email.App.Services;

public sealed class RabbitMqShoppingCartConsumer(
    IOptions<RabbitConnectionSettings> connectionSettings,
    IOptionsMonitor<RabbitQueue> namedQueues,
    IEmailService emailService,
    ILogger<RabbitMqShoppingCartConsumer> logger) : RabbitMqConsumer(connectionSettings, logger)
{

    private readonly RabbitQueue _queue = namedQueues.Get("EmailShoppingCart");


    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(0, stoppingToken).ConfigureAwait(false);
        if (!CreateChannelForQueue(_queue, out IModel? channel))
        {
            return;
        }

        EventingBasicConsumer consumer = new(channel);
        consumer.Received += ConsumerReceived;
        stoppingToken.ThrowIfCancellationRequested();
        channel.BasicConsume(_queue.QueueName, false, consumer);

        return;

        void ConsumerReceived(object? sender, BasicDeliverEventArgs e)
        {
            try
            {

                EmailMessage? emailMessage = JsonSerializer.Deserialize<EmailMessage>(e.Body.ToArray());
                if (emailMessage is null)
                {
                    Logger.LogError("Failed to parse email message from ShoppingCart queue");
                    return;
                }

                EmailLogEntry logEntry = emailMessage.ToLogCartEntry();
                emailService.Log(logEntry);
            }
            finally
            {
                channel.BasicAck(e.DeliveryTag, false);
            }
        }
    }
}

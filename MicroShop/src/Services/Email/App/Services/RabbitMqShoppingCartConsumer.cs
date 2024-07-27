using System.Text.Json;
using MicroShop.Services.Email.App.Infrastructure.Data;
using MicroShop.Services.Email.App.Models;
using MicroShop.Services.Email.App.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroShop.Services.Email.App.Services;

public sealed class RabbitMqShoppingCartConsumer(
    IOptions<RabbitConnectionSettings> connectionSettings,
    IOptions<RabbitSettings> settings,
    IDbContextFactory<AppDbContext> dbContextFactory,
    ILogger<RabbitMqShoppingCartConsumer> logger) : RabbitMqConsumer(connectionSettings, settings, logger)
{
    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(0, stoppingToken).ConfigureAwait(false);
        if (!CreateChannelForQueue("ShoppingCart", out IModel? channel, out string? queueName))
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

                EmailMessage? emailMessage = JsonSerializer.Deserialize<EmailMessage>(e.Body.ToArray());
                if (emailMessage is null)
                {
                    Logger.LogError("Failed to parse email message from ShoppingCart queue");
                    return;
                }

                EmailLogEntry logEntry = emailMessage.ToLogCartEntry();
                using AppDbContext dbContext = dbContextFactory.CreateDbContext();
                dbContext.EmailLogs.Add(logEntry);
                dbContext.SaveChanges();
            }
            finally
            {
                channel.BasicAck(e.DeliveryTag, false);
            }
        }
    }
}

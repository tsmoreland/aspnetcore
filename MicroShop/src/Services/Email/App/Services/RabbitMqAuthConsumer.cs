using System.Text.Json;
using MicroShop.Services.Email.App.Models;
using MicroShop.Services.Email.App.Models.DataTransferObjects;
using MicroShop.Services.Email.App.Services.Contracts;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroShop.Services.Email.App.Services;

public sealed class RabbitMqAuthConsumer(
    IOptions<RabbitConnectionSettings> connectionSettings,
    IOptionsMonitor<RabbitQueue> namedQueues,
    IEmailService emailService,
    ILogger<RabbitMqAuthConsumer> logger) : RabbitMqConsumer(connectionSettings, logger)
{
    private readonly RabbitQueue _queue = namedQueues.Get("RegisterUser");

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
                // TODO: changes this to registration dto, or an email object
                UserRegisteredNotificationMessage? message = JsonSerializer.Deserialize<UserRegisteredNotificationMessage>(e.Body.ToArray());
                if (message is null)
                {
                    return;
                }
                EmailLogEntry entry = BuildEmail(message.Name, message.EmailAddress, message.VerifyUrl);

                emailService.Log(entry);
            }
            finally
            {
                channel.BasicAck(e.DeliveryTag, false);
            }
        }
    }

    private static EmailLogEntry BuildEmail(string name, string emailAddress, string verifyLink)
    {
        string body = $"""
            <br/>
            <h3>Welcome {name} <{emailAddress}!</h3>
            <p>
            Please verify your Email address <a href="{verifyLink}">here</a>.
            </p>
            """;
        return new EmailLogEntry(emailAddress, body, DateTime.UtcNow);
    }
}

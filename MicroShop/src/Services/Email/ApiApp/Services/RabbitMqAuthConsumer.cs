using System.Text.Json;
using MicroShop.Services.Email.ApiApp.Infrastructure.Data;
using MicroShop.Services.Email.ApiApp.Models;
using MicroShop.Services.Email.ApiApp.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroShop.Services.Email.ApiApp.Services;

public sealed class RabbitMqAuthConsumer(
    IOptions<RabbitConnectionSettings> connectionSettings,
    IOptions<RabbitSettings> settings,
    IDbContextFactory<AppDbContext> dbContextFactory, // replace with RepositoryFactory
    ILogger<RabbitMqAuthConsumer> logger) : RabbitMqConsumer(connectionSettings, settings, logger)
{
    private bool _disposed;

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(0, stoppingToken).ConfigureAwait(false);
        if (!CreateChannelForQueue("RegisterUser", out IModel? channel, out string? queueName))
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
                // TODO: changes this to registration dto, or an email object
                UserRegisteredNotificationMessage? message = JsonSerializer.Deserialize<UserRegisteredNotificationMessage>(e.Body.ToArray());
                if (message is null)
                {
                    return;
                }

                // use e-mail service here to send the e-mail
                using AppDbContext dbContext = dbContextFactory.CreateDbContext();
                EmailLogEntry entry = BuildEmail(message.Name, message.EmailAddress, message.VerifyUrl);
                dbContext.Add(entry);
                dbContext.SaveChanges();
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

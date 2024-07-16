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
        stoppingToken.ThrowIfCancellationRequested();
        channel.BasicConsume(queue.QueueName, false, consumer);

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

    private EmailLogEntry BuildEmail(string name, string emailAddress, string verifyLink)
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

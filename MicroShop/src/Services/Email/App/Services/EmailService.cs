using MicroShop.Services.Email.App.Infrastructure.Data;
using MicroShop.Services.Email.App.Models;
using MicroShop.Services.Email.App.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Email.App.Services;

public sealed class EmailService(IDbContextFactory<AppDbContext> dbContextFactory, ILogger<EmailService> logger)
    : IEmailService
{
    /// <inheritdoc />
    public async ValueTask LogAsync(EmailLogEntry logEntry, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Writing message to {EmailAddress}", logEntry.EmailAddress);
        await using AppDbContext dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        dbContext.EmailLogs.Add(logEntry);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public void Log(EmailLogEntry logEntry)
    {
        logger.LogInformation("Writing message to {EmailAddress}", logEntry.EmailAddress);
        using AppDbContext dbContext = dbContextFactory.CreateDbContext();
        dbContext.EmailLogs.Add(logEntry);
        dbContext.SaveChanges();
    }
}

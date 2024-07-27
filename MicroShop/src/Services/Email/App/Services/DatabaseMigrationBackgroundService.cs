using MicroShop.Services.Email.App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Email.App.Services;

public sealed class DatabaseMigrationBackgroundService(
    IDbContextFactory<AppDbContext> databaseContextFactory,
    ILogger<DatabaseMigrationBackgroundService> logger) : BackgroundService
{
    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await using AppDbContext dbContext = await databaseContextFactory.CreateDbContextAsync(stoppingToken).ConfigureAwait(false);
            if ((await dbContext.Database.GetPendingMigrationsAsync(stoppingToken).ConfigureAwait(false)).Any())
            {
                await dbContext.Database.MigrateAsync(stoppingToken).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred performing database migration");
        }
    }
}

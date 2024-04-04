using MicroShop.Services.Auth.AuthApiApp;
using MicroShop.Services.Auth.AuthApiApp.Infrrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    WebApplicationBuilder builder = WebApplication
        .CreateBuilder(args)
        .Configure();

    builder.Host
        .UseSerilog((context, loggerConfiguration) => loggerConfiguration
#if DEBUG
            .WriteTo.Console()
#endif
            .ReadFrom.Configuration(context.Configuration));

    WebApplication app = builder
        .Build()
        .ConfigurePipeline();

    await ApplyMigrations(app.Services);
    await app.RunAsync();
}
catch (HostAbortedException) when (args is ["--applicationName", _])
{
    // expected during EF Core Migration
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shutdown complete.");
    Log.CloseAndFlush();
}

return;

static async Task ApplyMigrations(IServiceProvider services)
{
    using IServiceScope scope = services.CreateScope();
    AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
    {
        await dbContext.Database.MigrateAsync();
    }
}

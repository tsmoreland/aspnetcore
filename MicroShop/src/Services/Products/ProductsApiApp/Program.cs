using MicroShop.Services.Products.ProductsApiApp;
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
    app.Run();
}
catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException" )
{
    if (ex is HostAbortedException && args is ["--applicationName", _])
    {
        return; // exception during EF Migration
    }

    // https://github.com/dotnet/runtime/issues/60600 for StopTheHostException
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
    /*
    AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
    {
        await dbContext.Database.MigrateAsync();
    }
    */
    await ValueTask.CompletedTask;
}

using MicroShop.Services.Email.ApiApp;
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

    app.Run();
}
catch (HostAbortedException) when (args is ["--applicationName", _])
{
    // expected during EF Core Add Migration
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


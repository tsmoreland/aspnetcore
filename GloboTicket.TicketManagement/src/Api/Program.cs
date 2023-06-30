using GloboTicket.TicketManagement.Api;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("GloboTicket API Starting...");

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    builder.Host
        .UseSerilog((context, loggerConfiguration) => loggerConfiguration
            .WriteTo.Console()
            .ReadFrom.Configuration(context.Configuration));


    WebApplication app = builder
        .ConfigureServices()
        .ConfigurePipeline();

    app.UseSerilogRequestLogging();

#if DEBUG
    await app.ResetDatabaseAsync();
#endif

    app.Run();
}
catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException")
{
    // https://github.com/dotnet/runtime/issues/60600 for StopTheHostException
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}


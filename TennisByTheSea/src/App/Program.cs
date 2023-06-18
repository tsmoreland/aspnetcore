using Serilog;
using TennisByTheSea.App;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    WebApplicationBuilder builder = WebApplication
        .CreateBuilder(args)
        .ConfigureBuilder();

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
catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException")
{
    // https://github.com/dotnet/runtime/issues/60600 for StopTheHostException
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shutdown complete.");
    Log.CloseAndFlush();
}

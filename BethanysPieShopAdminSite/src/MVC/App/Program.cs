using System.Globalization;
using BethanysPieShop.MVC.App;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication
        .CreateBuilder(args)
        .ConfigureBuilder();

    builder.Host
        .UseSerilog((context, loggerConfiguration) => loggerConfiguration
#if DEBUG
            .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
#endif
            .ReadFrom.Configuration(context.Configuration));

    var app = builder
        .Build()
        .ConfigurePipeline();

    await app.RunAsync().ConfigureAwait(true);
}
catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException")
{
    // https://github.com/dotnet/runtime/issues/60600 for StopTheHostException
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shutdown complete.");
    await Log.CloseAndFlushAsync().ConfigureAwait(true);
}

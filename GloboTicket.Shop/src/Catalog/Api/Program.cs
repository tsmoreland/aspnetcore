using System.Globalization;
using GloboTicket.Shop.Catalog.Api;
using Serilog;
using Serilog.Formatting.Display;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(new MessageTemplateTextFormatter("[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", CultureInfo.InvariantCulture))
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication
        .CreateBuilder(args)
        .ConfigureServices();

    var app = builder
        .Build()
        .Configure();

    await app.RunAsync().ConfigureAwait(true);
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


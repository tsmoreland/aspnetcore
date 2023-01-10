using GloboTicket.TicketManagement.Api;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("GloboTicket API Starting...");

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

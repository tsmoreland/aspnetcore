using CarInventory.Api;
using CarInventory.Infrastructure.Persistence;
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

    using (IServiceScope scope = app.Services.CreateScope())
    {
        CarsDbContext dbContext = scope.ServiceProvider.GetRequiredService<CarsDbContext>();
        dbContext.Database.Migrate();
    }

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


/*
app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

*/


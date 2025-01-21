using System.Globalization;
using System.Text.Json;
using GloboTicket.Shop.Catalog.Api.Services;
using GloboTicket.Shop.Catalog.Application;
using GloboTicket.Shop.Catalog.Infrastructure;
using GloboTicket.Shop.Shared.Api;
using GloboTicket.Shop.Shared.Api.Filters;
using GloboTicket.Shop.Shared.Contracts.Hosting;
using Serilog;
using Serilog.Formatting.Display;

namespace GloboTicket.Shop.Catalog.Api;

internal static class Startup
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Configuration.AddEnvironmentVariables();

        builder.WebHost
            .ConfigureKestrel(o => o.AddServerHeader = false);

        builder.Host
            .UseSerilog(static (context, loggerConfiguration) => loggerConfiguration
                .WriteTo.Console(new MessageTemplateTextFormatter("[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", CultureInfo.InvariantCulture))
                .ReadFrom.Configuration(context.Configuration));

        var services = builder.Services;

        services
            .AddHealthChecks();

        services
            .AddControllers(options => options.Filters.Add(new AddModelStateFeatureFilter()))
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition =
                    System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

        services
            .AddProblemDetails()
            .AddSwaggerGen()
            .AddSingleton<IHostEnvironmentFacade, HostEnvironmentFacade>()
            .AddCatalogApplicationServices()
            .AddCatalogInfrastructure();

        return builder;
    }

    public static WebApplication Configure(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseSerilogRequestLogging();
        app.AddValidationExceptionHandler();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapHealthChecks("/health");
        app.MapControllers();

        return app;
    }
}

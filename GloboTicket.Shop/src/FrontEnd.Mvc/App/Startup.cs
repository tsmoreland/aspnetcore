using System.Globalization;
using System.Text.Json;
using GloboTicket.FrontEnd.Mvc.App.HealthChecks;
using GloboTicket.FrontEnd.Mvc.App.Models;
using GloboTicket.FrontEnd.Mvc.App.Services.ConcertCatalog;
using GloboTicket.FrontEnd.Mvc.App.Services.Ordering;
using GloboTicket.FrontEnd.Mvc.App.Services.ShoppingBasket;
using GloboTicket.FrontEnd.Mvc.App.Services.ShoppingBasket.InMemory;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

namespace GloboTicket.FrontEnd.Mvc.App;

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
                .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
                .ReadFrom.Configuration(context.Configuration));

        var services = builder.Services;

        services
            .AddControllersWithViews()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition =
                    System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

        services
            .AddHttpClient<IConcertCatalogService, ConcertCatalogService>((provider, client) => client.BaseAddress = new Uri(provider.GetService<IConfiguration>()?["ApiConfigs:ConcertCatalog:Uri"] ?? throw new InvalidOperationException("Missing config")));
        services
            .AddHttpClient<IOrderSubmissionService, HttpOrderSubmissionService>((provider, client) => client.BaseAddress = new Uri(provider.GetService<IConfiguration>()?["ApiConfigs:Ordering:Uri"] ?? throw new InvalidOperationException("Missing config")));

        services
            .AddHealthChecks()
            .AddCheck<SlowDependencyHealthCheck>("SlowDepdenency", tags: ["ready"])
            .AddProcessAllocatedMemoryHealthCheck(maximumMegabytesAllocated: 500);

        services
            .AddSingleton<Settings>()
            .AddSingleton<IShoppingBasketService, InMemoryShoppingBasketService>()
            .AddApplicationInsightsTelemetry();

        return builder;
    }

    public static WebApplication Configure(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);
        app.UseSerilogRequestLogging();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/AspNetCore-hsts.
            app.UseHsts();
        }

        // Turning this off to simplify the running in Kubernetes demo
        // app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=ConcertCatalog}/{action=Index}/{id?}");

        app.MapHealthChecks("/health/ready",
            new HealthCheckOptions()
            {
                Predicate = reg => reg.Tags.Contains("ready"),
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            });

        app.MapHealthChecks("/health/lively",
            new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            });
        return app;
    }
}

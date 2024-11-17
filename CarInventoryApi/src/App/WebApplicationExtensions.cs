using CarInventory.App.Configuration;
using CarInventory.Cars.Api;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CarInventory.App;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.UseResponseCompression();
        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi("/api/{documentName}/openapi.json");
            app.UseSwaggerUI(options => ConfigureSwaggerUserInterface(options, app));
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseRouting();
        app.UseRateLimiter();
        app.UseAuthorization();

        app.MapHealthChecks("/api/health", HealthChecksConfiguration.GetOptions())
            .AllowAnonymous();

        app.MapGroup("/api/v{version}/cars")
            .MapCarsApi("/api/v1/cars",
                addPolicies: ["application_admin"],
                getPolicies: ["application_client"],
                updatePolicies: ["application_admin"],
                deletePolicies: ["application_admin"]);

        return app;
    }

    private static void ConfigureSwaggerUserInterface(SwaggerUIOptions options, WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var openApiOptions = scope.ServiceProvider.GetServices<IOptions<OpenApiOptions>>();
        foreach (var apiOptions in openApiOptions.Select(o => o.Value))
        {
            options.SwaggerEndpoint($"/api/{apiOptions.DocumentName}/openApi.json", apiOptions.DocumentName);
        }
    }
}

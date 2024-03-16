using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;
using CarInventory.App.Configuration;
using CarInventory.App.RouteGroups;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
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
            app.UseSwagger(ConfigureSwagger);
            app.UseSwaggerUI(options => ConfigureSwaggerUserInterface(options, app.DescribeApiVersions()));
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseRouting();
        app.UseRateLimiter();
        app.UseAuthorization();

        app.MapHealthChecks("/api/health", HealthChecksConfiguration.GetOptions())
            .AllowAnonymous();

        IVersionedEndpointRouteBuilder routeBuilder = app.NewVersionedApi();
        routeBuilder.MapGroup("/api/v{version:apiVersion}/cars")
            .MapCars();

        return app;
    }
    private static void ConfigureSwagger(SwaggerOptions options)
    {
        options.PreSerializeFilters.Add(static (swaggerDoc, httpReq) =>
        {
            swaggerDoc.Servers = 
            [
                new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" }
            ];
        });
    }

    private static void ConfigureSwaggerUserInterface(SwaggerUIOptions options, IEnumerable<ApiVersionDescription> apiVersions)
    {
        foreach (ApiVersionDescription description in apiVersions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
        }
    }
}

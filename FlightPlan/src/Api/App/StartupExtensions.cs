using System.Text.Json.Serialization;
using FlightPlan.Api.App.Authentication;
using FlightPlan.Api.App.Infrastructure;
using FlightPlan.Persistence;
using Microsoft.AspNetCore.Authentication;
using TSMoreland.Text.Json.NamingStrategies;
using TSMoreland.Text.Json.NamingStrategies.Strategies;

namespace FlightPlan.Api.App;

internal static class StartupExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (builder.Environment.IsDevelopment()) builder.Services.AddProblemDetails();
        else builder.Services.AddProblemDetails(static options => options.CustomizeProblemDetails = static ctx => ctx.ProblemDetails.Extensions.Clear());

        builder.Services.AddOpenApi(options => options
            .UseDocumentInfo("Flight Plan API", "v1")
            .UseServerUrls("https://localhost")
            .UseBasicAuthAuthentication()
            .AddDocumentTransformer<BasicSecuritySchemeTransformer>());

        builder.Services
            .AddControllers()
            .AddJsonOptions(static options =>
            {
                var serializeOptions = options.JsonSerializerOptions;
                serializeOptions.PropertyNamingPolicy = JsonStrategizedNamingPolicy.SnakeCase;
                serializeOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                serializeOptions.Converters
                    .Add(new JsonStrategizedStringEnumConverterFactory(new SnakeCaseEnumNamingStrategy()));

            });

        builder.Services
            .AddTransient<IUserService, UserService>()
            .AddCors()
            .AddPersistence(builder.Configuration);

        builder.Services
            .AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentiation", null);

        builder.Services
            .AddAuthorizationBuilder()
            .AddPolicy("default", p => p.RequireAuthenticatedUser());

        return builder;
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app
                .MapOpenApi("/api/flight_plan/{documentName}/openapi.json");
            app.UseSwaggerUI(static options => options.SwaggerEndpoint("/api/flight_plan/v1/openapi.json", "Flight Plan API"));
        }

        app.UseCors(config =>
            config
                .AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowAnyMethod());

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseRouting();
        app.UseRateLimiter();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}

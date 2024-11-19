using System.IO.Compression;
using System.Security.Authentication;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using CarInventory.App.Configuration;
using CarInventory.App.ErrorHandlers;
using CarInventory.Cars.Application.Features.Commands.Add;
using CarInventory.Cars.Infrastructure;
using CarInventory.Cars.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Identity.Web;
using AspNetCoreHttp = Microsoft.AspNetCore.Http;
using AspNetCoreMvc = Microsoft.AspNetCore.Mvc;
using SerializerContext = CarInventory.App.Configuration.SerializerContext;

namespace CarInventory.App;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var services = builder.Services;
        var environment = builder.Environment;

        builder.WebHost
            .UseKestrel(static (ctx, options) =>
            {
                options
                    .Configure(ctx.Configuration.GetSection("Kestrel"))
                    .Endpoint("HTTPS", listenOptions => listenOptions.HttpsOptions.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13);
                options.AddServerHeader = false;
            });

        services
            .AddExceptionHandler<ValidationExceptionHandler>()
            .AddExceptionHandler<BadHttpRequestExceptionHandler>();

        services.AddMediatR(options => options.RegisterServicesFromAssemblies(typeof(CarsDbContext).Assembly, typeof(AddCommandHandler).Assembly, typeof(Program).Assembly));

        if (environment.IsDevelopment())
        {
            services
                .AddProblemDetails();

            services
                .AddOpenApi(options =>
                {
                    options
                        .UseDocumentInfo("Cars Inventory API", "v1")
                        .UseServerUrls("https://localhost")
                        .UseJwtBearerAuthentication();
                });
        }
        else
        {
            services
                .AddProblemDetails(static options => options.CustomizeProblemDetails = static ctx => ctx.ProblemDetails.Extensions.Clear());
        }

        var healthChecksBuilder = services.AddHealthChecks();

        services
            .AddAuthorizationBuilder()
            .AddPolicy("application_client", policy => policy
                .RequireRole("role.read.cars"))
            .AddPolicy("application_admin", policy => policy
                .RequireRole("role.read.cars", "role.crud.cars"));

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration);

        services
            .ConfigureJsonOptions()
            .AddRateLimiter(RateLimitConfiguration.Configure)
            //.AddEndpointsApiExplorer()
            .AddResponseCompressionWithBrotliAndGzip();

        services
            .AddInfrastructureServices(configuration, healthChecksBuilder, environment);

        return builder;
    }
    private static IServiceCollection AddResponseCompressionWithBrotliAndGzip(this IServiceCollection services)
    {
        return services
            .AddResponseCompression(static options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            })
            .Configure<BrotliCompressionProviderOptions>(static options => options.Level = CompressionLevel.Optimal);
    }
    private static IServiceCollection ConfigureJsonOptions(this IServiceCollection services)
    {
        return services
            .Configure<AspNetCoreMvc.JsonOptions>(ConfigureMvcJsonOptions)
            .Configure<AspNetCoreHttp.Json.JsonOptions>(ConfigureHttpJsonOptions);

        static void ConfigureMvcJsonOptions(AspNetCoreMvc.JsonOptions options)
        {
            ConfigureJsonSerializerOptions(options.JsonSerializerOptions);
        }
        static void ConfigureHttpJsonOptions(AspNetCoreHttp.Json.JsonOptions options)
        {
            ConfigureJsonSerializerOptions(options.SerializerOptions);
        }

    }
    private static void ConfigureJsonSerializerOptions(JsonSerializerOptions options)
    {
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;

        options.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false));

        options.PropertyNamingPolicy = SnakeCaseJsonNamingPolicy.Instance;
        options.TypeInfoResolver = JsonTypeInfoResolver.Combine(
            SerializerContext.Default,
            new DefaultJsonTypeInfoResolver());
    }
}

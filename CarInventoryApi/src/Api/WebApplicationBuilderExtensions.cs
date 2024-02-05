using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using System.Security.Authentication;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Asp.Versioning.ApiExplorer;
using CarInventory.Api.Configuration;
using CarInventory.Api.ErrorHandlers;
using CarInventory.Application.Features.Cars.Commands.Add;
using CarInventory.Infrastructure;
using CarInventory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Swashbuckle.AspNetCore.SwaggerGen;
using AspNetCoreHttp = Microsoft.AspNetCore.Http;
using AspNetCoreMvc = Microsoft.AspNetCore.Mvc;

namespace CarInventory.Api;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        IConfiguration configuration = builder.Configuration;
        IServiceCollection services = builder.Services;
        IHostEnvironment environment = builder.Environment;

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
                .AddProblemDetails()
                .AddSwaggerGen()
                .AddTransient<IConfigureOptions<SwaggerGenOptions>>(provider => new SwaggerConfiguration(provider.GetRequiredService<IApiVersionDescriptionProvider>()));
        }
        else
        {
            services
                .AddProblemDetails(static options => options.CustomizeProblemDetails = static ctx => ctx.ProblemDetails.Extensions.Clear());
        }

        IHealthChecksBuilder healthChecksBuilder = services.AddHealthChecks();

        services
            .AddAuthorizationBuilder()
            .AddPolicy("application_client", policy => policy
                .RequireRole("read.cars"))
            .AddPolicy("application_admin", policy => policy
                .RequireRole("read.cars", "write.cars"));

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration);

        services
            .ConfigureJsonOptions()
            .AddRateLimiter(RateLimitConfiguration.Configure)
            .AddEndpointsApiExplorer()
            .AddVersioning()
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
    private static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        services
            .AddApiVersioning(static options =>
            {
                options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1);
                options.ApiVersionReader = new Asp.Versioning.UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(static options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.FormatGroupName = (group, version) => $"{group} - {version}";
                options.SubstituteApiVersionInUrl = true;
            });
        return services;
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

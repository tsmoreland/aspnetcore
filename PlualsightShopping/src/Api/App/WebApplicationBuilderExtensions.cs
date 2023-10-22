//
// Copyright © 2023 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using PluralsightShopping.Api.App.Configuration;
using PluralsightShopping.Api.Application;
using Swashbuckle.AspNetCore.SwaggerGen;
using AspNetCoreHttp = Microsoft.AspNetCore.Http;
using AspNetCoreMvc = Microsoft.AspNetCore.Mvc;

namespace PluralsightShopping.Api.App;

internal static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

        IServiceCollection services = builder.Services;
        IConfiguration configuration = builder.Configuration;
        IHostEnvironment environment = builder.Environment;

        if (environment.IsDevelopment())
        {
            services.AddProblemDetails();
        }
        else
        {
            services
                .AddProblemDetails(static options => options.CustomizeProblemDetails = static ctx => ctx.ProblemDetails.Extensions.Clear());
        }

        services
            .AddMediatR(options => options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        IHealthChecksBuilder healthChecksBuilder = services.AddHealthChecks();

        if (environment.IsDevelopment())
        {
            // Add security headers
        }
        else
        {
            // Add security headers
        }

        services
            .AddDataProtection();

        services
            .AddApiRateLimiting(configuration)
            .AddResponseCompressionWithBrotliAndGzip()
            .AddHttpContextAccessor()
            .AddIdentity()
            .AddVersioning()
            .AddOpenApiGen()
            .ConfigureJsonOptions()
            .AddControllers();

        services
            .AddApplicationServices();

        return builder;
    }

    private static IServiceCollection AddApiRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .Configure<ApiRateLimitOptions>(configuration.GetSection(ApiRateLimitOptions.SectionName))
            .AddRateLimiter(ApiRateLimitOptions.Configure);
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

    private static IServiceCollection AddOpenApiGen(this IServiceCollection services)
    {
        services
            .AddEndpointsApiExplorer()
            //.AddTransient<IConfigureOptions<SwaggerGenOptions>>(provider => new SwaggerConfiguration(provider.GetRequiredService<IApiVersionDescriptionProvider>(), provider))
            .AddSwaggerGen();
        return services;
    }

    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        /*
        services.TryAddScoped<SignInManager<User>>();
        services.TryAddScoped<RoleManager<Role>>();

        services
            .Configure<DataProtectionTokenProviderOptions>(static options => options.TokenLifespan = TimeSpan.FromHours(1))
            .AddScoped<IUserClaimsPrincipalFactory<User>, AdditionalUserClaimsPrincipalFactory>();
        */

        /*
        services
            .AddIdentityCore<User>(static identityOptions =>
            {
                // Password settings.
                identityOptions.Password.RequireDigit = false;
                identityOptions.Password.RequireLowercase = false;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Password.RequiredLength = 12;
                identityOptions.Password.RequiredUniqueChars = 1;

                identityOptions.SignIn.RequireConfirmedPhoneNumber = false;
                identityOptions.SignIn.RequireConfirmedEmail = false;
                identityOptions.SignIn.RequireConfirmedAccount = false;

                // these are defaults just adding to be explicit
                identityOptions.Lockout.MaxFailedAccessAttempts = 5;
                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                identityOptions.Lockout.AllowedForNewUsers = true;

                identityOptions.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                identityOptions.User.RequireUniqueEmail = true;
            });
        */

        // Create a new builder so we can specify ApiRole is in use as well
        /* -- if/when we have a database
        new IdentityBuilder(typeof(User), typeof(Role), services)
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AppDbContext>();
        */

        return services;
    }

    public static IServiceCollection ConfigureJsonOptions(this IServiceCollection services)
    {
        return services
            .Configure<AspNetCoreMvc.JsonOptions>(ConfigureMvcJsonOptions)
            .Configure<AspNetCoreHttp.Json.JsonOptions>(ConfigureHttpJsonOptions);
    }

    private static void ConfigureMvcJsonOptions(AspNetCoreMvc.JsonOptions options)
    {
        ConfigureJsonSerializerOptions(options.JsonSerializerOptions);
    }

    private static void ConfigureHttpJsonOptions(AspNetCoreHttp.Json.JsonOptions options)
    {
        ConfigureJsonSerializerOptions(options.SerializerOptions);
    }

    private static void ConfigureJsonSerializerOptions(JsonSerializerOptions options)
    {
        options.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;

        options.PropertyNamingPolicy = SnakeCaseJsonNamingPolicy.Instance;
        options.TypeInfoResolver = JsonTypeInfoResolver.Combine(
            // custom serializer goes here
            new DefaultJsonTypeInfoResolver());
    }
}

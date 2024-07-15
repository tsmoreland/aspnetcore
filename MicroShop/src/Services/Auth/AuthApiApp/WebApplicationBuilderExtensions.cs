using System.Security.Authentication;
using MicroShop.Integrations.MessageBus;
using MicroShop.Services.Auth.AuthApiApp.Infrrastructure.Data;
using MicroShop.Services.Auth.AuthApiApp.Models;
using MicroShop.Services.Auth.AuthApiApp.Services;
using MicroShop.Services.Auth.AuthApiApp.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

namespace MicroShop.Services.Auth.AuthApiApp;

internal static class WebApplicationBuilderExtensions
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

        if (environment.IsDevelopment())
        {
            services.AddProblemDetails();
        }
        else
        {
            services
                .AddProblemDetails(static options => options.CustomizeProblemDetails = static ctx => ctx.ProblemDetails.Extensions.Clear());
        }

        services.AddAuthorization();

        services.Configure<ApiJwtOptions>(configuration.GetSection("ApiJwtOptions"));

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AppConnection")));
        services
            .AddIdentity<AppUser, AppRole>(static options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 12;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddResponseCompression(static options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            })
            .Configure<BrotliCompressionProviderOptions>(static options => options.Level = System.IO.Compression.CompressionLevel.Optimal);

        services
            .AddScoped<IJwtTokenGenerator, JwtTokenGenerator>()
            .AddMessageBus(configuration)
            .Configure<RabbitConnectionSettings>(configuration.GetSection("RabbitMQConnection"))
            .AddScoped<IAuthService, AuthService>();

        return builder;
    }
}

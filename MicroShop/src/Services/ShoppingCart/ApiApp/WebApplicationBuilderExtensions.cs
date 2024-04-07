using System.Security.Authentication;
using System.Text;
using System.Threading.RateLimiting;
using MicroShop.Services.ShoppingCart.ApiApp.Infrastructure.Data;
using MicroShop.Services.ShoppingCart.ApiApp.Services;
using MicroShop.Services.ShoppingCart.ApiApp.Services.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace MicroShop.Services.ShoppingCart.ApiApp;

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

        services
            .AddDbContextFactory<AppDbContext>(dbOptions =>
                dbOptions.UseSqlServer(configuration.GetConnectionString("AppConnection"), sqlOptions => sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.ToString())));
        services
            .AddScoped(static provider => provider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext());

        services
            .AddOptions()
            .AddMemoryCache()
            .AddRateLimiter(static _ => _.AddFixedWindowLimiter(policyName: "fixedWindow",
                options =>
                {
                    options.PermitLimit = 500;
                    options.Window = TimeSpan.FromSeconds(1);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 5;
                }));

        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(static options =>
            {
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization token as `Bearer JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme,
                            }
                        },
                        []
                    }
                });
            })
            .AddResponseCompression(static options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            })
            .Configure<BrotliCompressionProviderOptions>(static options => options.Level = System.IO.Compression.CompressionLevel.Optimal);

        services
            .AddHostedService<DatabaseMigrationBackgroundService>()
            .AddScoped<ICartService, CartService>()
            .AddScoped<IProductService, ProductService>()
            .AddAuthorization();

        services
            .AddAuthentication(static options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                string issuer = configuration["ApiJwtOptions:Issuer"] ?? string.Empty;
                string audience = configuration["ApiJwtOptions:Audience"] ?? string.Empty;
                byte[] secret = Encoding.UTF8.GetBytes(configuration["ApiJwtOptions:Secret"] ?? string.Empty);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                };
            });

        services.AddHttpClient("ProductsApi", httpClient =>
        {
            string productsApiUrl = configuration["ServiceUrls:ProductsApi"] ?? throw new KeyNotFoundException("Missing entry in appsettings");
            httpClient.BaseAddress = new Uri(productsApiUrl);
        });

        return builder;
    }
}

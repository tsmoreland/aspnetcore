using System.Security.Authentication;

namespace MicroShop.Services.Coupons.CouponApiApp;

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
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

        return builder;
    }
}

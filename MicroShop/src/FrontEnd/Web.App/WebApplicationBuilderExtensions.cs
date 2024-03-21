using System.Net.Mime;
using System.Security.Authentication;
using MicroShop.Web.App.Services;
using MicroShop.Web.App.Services.Contracts;
using Microsoft.AspNetCore.ResponseCompression;

namespace MicroShop.Web.App;

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

        services
            .AddResponseCompression(static options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            })
            .Configure<BrotliCompressionProviderOptions>(static options => options.Level = System.IO.Compression.CompressionLevel.Optimal);


        services
            .AddScoped<IBaseService, BaseService>()
            .AddScoped<ICouponService, CouponService>();

        services.AddControllersWithViews();
        services.AddHttpContextAccessor();
        services.AddHttpClient();
        services.AddHttpClient("CouponsApi", httpClient =>
        {
            string couponApiUrl = configuration["ServiceUrls:CouponApi"] ?? throw new KeyNotFoundException("Missing entry in appsettings");
            httpClient.BaseAddress = new Uri(couponApiUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", MediaTypeNames.Application.Json);
        });

        return builder;
    }
}

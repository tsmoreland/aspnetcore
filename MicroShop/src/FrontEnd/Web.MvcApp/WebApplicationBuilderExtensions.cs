using System.Net.Mime;
using System.Security.Authentication;
using MicroShop.Web.MvcApp.Services;
using MicroShop.Web.MvcApp.Services.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.ResponseCompression;

namespace MicroShop.Web.MvcApp;

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
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<ITokenProvider, TokenProvider>()
            .AddScoped<ICouponService, CouponService>()
            .AddScoped<IProductService, ProductService>();

        services
            .AddHttpContextAccessor()
            .AddHttpClient()
            .AddControllersWithViews();

        services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(static options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.LoginPath = "/auth/login";
                options.AccessDeniedPath = "/auth/AccessDenied";
            });

        services.AddHttpClient("CouponsApi", httpClient =>
        {
            string couponApiUrl = configuration["ServiceUrls:CouponApi"] ?? throw new KeyNotFoundException("Missing entry in appsettings");
            httpClient.BaseAddress = new Uri(couponApiUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", MediaTypeNames.Application.Json);
        });

        services.AddHttpClient("AuthApi", httpClient =>
        {
            string authApiUrl = configuration["ServiceUrls:AuthApi"] ?? throw new KeyNotFoundException("Missing entry in appsettings");
            httpClient.BaseAddress = new Uri(authApiUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", MediaTypeNames.Application.Json);
        });
        services.AddHttpClient("ProductsApi", httpClient =>
        {
            string productsApiUrl = configuration["ServiceUrls:ProductsApi"] ?? throw new KeyNotFoundException("Missing entry in appsettings");
            httpClient.BaseAddress = new Uri(productsApiUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", MediaTypeNames.Application.Json);
        });


        return builder;
    }
}

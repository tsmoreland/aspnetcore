﻿using System.Security.Authentication;
using MicroShop.Services.Coupons.CouponApiApp.Infrastructure.Data;
using MicroShop.Services.Coupons.CouponApiApp.Models;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

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

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AppConnection"), sqlOptions => sqlOptions.MigrationsAssembly(typeof(Coupon).Assembly.ToString())));

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
            .AddAuthorizationBuilder();

        services
            .AddAuthentication();

        return builder;
    }

}
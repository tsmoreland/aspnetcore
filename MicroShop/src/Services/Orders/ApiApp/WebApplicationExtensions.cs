using MicroShop.Services.Orders.ApiApp.Api.Commands;
using Serilog;
using Stripe;

namespace MicroShop.Services.Orders.ApiApp;

internal static class WebApplicationExtensions
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.UseResponseCompression();
        app.UseExceptionHandler();
        app.UseRateLimiter();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        StripeConfiguration.ApiKey = app.Configuration["Stripe::SecretKey"];

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();

        return app.UseMinimalApi();
    }

    private static WebApplication UseMinimalApi(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("api/orders");

        group
            .MapPost("", CreateOrder.Handle)
            .RequireAuthorization()
            .WithOpenApi()
            .WithName(nameof(CreateOrder));

        group
            .MapPost("stripe", CreateStripeSession.Handle)
            .RequireAuthorization()
            .WithOpenApi()
            .WithName(nameof(CreateStripeSession));

        return app;
    }
}

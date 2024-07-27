using MicroShop.Services.ShoppingCart.App.Api;
using Serilog;

namespace MicroShop.Services.ShoppingCart.App;

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

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();

        app.MapGroup("/api/cart").MapShoppingCartApi();

        return app;
    }

}

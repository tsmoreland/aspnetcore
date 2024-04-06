using MicroShop.Services.ShoppingCart.ApiApp.Api;
using Serilog;

namespace MicroShop.Services.ShoppingCart.ApiApp;

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

        app.MapControllers();
        app.MapGroup("/api/cart").MapShoppingCartApi();

        return app;
    }

}

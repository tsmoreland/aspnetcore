using MicroShop.Services.Products.App.Api;
using Serilog;

namespace MicroShop.Services.Products.App;

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

        app.UseStaticFiles();

        app.MapControllers();
        app.MapGroup("/api/products").MapProductsApi();

        return app;
    }
}

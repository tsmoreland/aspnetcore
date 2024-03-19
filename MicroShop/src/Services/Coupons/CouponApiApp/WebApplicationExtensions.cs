using MicroShop.Services.Coupons.CouponApiApp.Api;
using Serilog;

namespace MicroShop.Services.Coupons.CouponApiApp;

internal static class WebApplicationExtensions
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.UseResponseCompression();
        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();


        app.MapGroup("/api/cars").MapCouponsApi();

        return app;
    }
}

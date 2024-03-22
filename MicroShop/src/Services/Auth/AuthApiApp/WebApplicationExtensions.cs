using MicroShop.Services.Auth.AuthApiApp.Api;
using Serilog;

namespace MicroShop.Services.Auth.AuthApiApp;

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


        app.MapGroup("/api/auth")
            .MapAuthApi();

        return app;
    }
}

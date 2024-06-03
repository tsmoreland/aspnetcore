using Ocelot.Middleware;

namespace MicroShop.Gateway.App;

internal static class WebApplicationExtensions
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseOcelot();
        return app;
    }

}

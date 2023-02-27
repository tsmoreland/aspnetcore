// 
// Copyright © 2023 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using GloboTicket.Shop.Catalog.Api.Services;
using GloboTicket.Shop.Catalog.Application;
using GloboTicket.Shop.Catalog.Infrastructure;
using GloboTicket.Shop.Shared.Api;
using GloboTicket.Shop.Shared.Api.Filters;
using GloboTicket.Shop.Shared.Contracts.Hosting;
using Serilog;

namespace GloboTicket.Shop.Catalog.Api;

internal static class Startup
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Configuration.AddEnvironmentVariables();

        builder.WebHost
            .ConfigureKestrel(o =>
            {
                o.AddServerHeader = false;
            });

        builder.Host
            .UseSerilog(static (context, loggerConfiguration) => loggerConfiguration
                .WriteTo.Console()
                .ReadFrom.Configuration(context.Configuration));

        IServiceCollection services = builder.Services;

        services
            .AddHealthChecks();

        services
            .AddControllers(options => options.Filters.Add(new AddModelStateFeatureFilter()));

        services
            .AddProblemDetails()
            .AddSwaggerGen()
            .AddSingleton<IHostEnvironmentFacade, HostEnvironmentFacade>()
            .AddCatalogApplicationServices()
            .AddCatalogInfrastructure();

        return builder;
    }

    public static WebApplication Configure(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseSerilogRequestLogging();
        app.AddValidationExceptionHandler();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapHealthChecks("/health");
        app.MapControllers();

        return app;
    }
}

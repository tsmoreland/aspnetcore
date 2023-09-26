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
//

using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;
using Microsoft.OpenApi.Models;
using PlualsightShopping.Shared.DataTransferObjects;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace PluralsightShopping.Api.App;

internal static class WebApplicationExtensions
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseSerilogRequestLogging();
        app.UseResponseCompression();
        app.UseExceptionHandler();
        // app.AddValidationExceptionHandler(); // Custom Validation exception handler/translator

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(ConfigureSwagger);
            app.UseSwaggerUI(options => ConfigureSwaggerUI(options, app.DescribeApiVersions()));
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseRouting();
        app.UseRateLimiter();
        app.UseAuthorization();

        // Add Health Checks

        IVersionedEndpointRouteBuilder version1 = app.NewVersionedApi();

        // hard coded data for now
        string[] products = new[] { "Hiking Boots", "Tent", "Jacket", "Hiking Poles" };

        RouteGroupBuilder productGroup = version1.MapGroup("/api/v{version:apiVersion}/products")
            .WithTags("Products")
            .HasApiVersion(1);
        productGroup
            .MapGet("/", () => products.Select(p => new Product(p)))
            .WithName("GetAllProducts");

        return app;
    }

    private static void ConfigureSwagger(SwaggerOptions options)
    {
        options.PreSerializeFilters.Add(ConfigureSwaggerServers);

        return;

        static void ConfigureSwaggerServers(OpenApiDocument document, HttpRequest request) =>
            document.Servers = new List<OpenApiServer> { new () { Url = $"{request.Scheme}://{request.Host.Value}" } };
    }

    private static void ConfigureSwaggerUI(SwaggerUIOptions options, IEnumerable<ApiVersionDescription> apiVersions)
    {
        foreach (ApiVersionDescription description in apiVersions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
        }
    }
}

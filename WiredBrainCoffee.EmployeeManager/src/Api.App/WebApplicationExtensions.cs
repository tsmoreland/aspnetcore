// 
// Copyright © 2022 Terry Moreland
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

using System.Net.Mime;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using WiredBrainCoffee.EmployeeManager.Infrastructure;

namespace WiredBrainCoffee.EmployeeManager.Api.App;

public static class WebApplicationExtensions
{
    public static async Task EnsureDatabaseIsMigrated(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        await using EmployeeManagerDbContext context = scope.ServiceProvider.GetRequiredService<EmployeeManagerDbContext>();
        await context.Database.MigrateAsync();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger(ConfigureSwagger);
            app.UseSwaggerUI(options => ConfigureSwaggerUI(options, app.DescribeApiVersions()));
        }
        else
        {
            app.UseExceptionHandler();
            app.UseStatusCodePages();
        }

        app.UseHttpsRedirection();

        string[] summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

        IVersionedEndpointRouteBuilder employeeApi = app.NewVersionedApi();

        employeeApi
            .MapGet("/weatherforecast", () =>
            {
                WeatherForecast[] forecast = Enumerable.Range(1, 5).Select(index =>
                        new WeatherForecast
                        (
                            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            Random.Shared.Next(-20, 55),
                            summaries[Random.Shared.Next(summaries.Length)]
                        ))
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi(static op => new OpenApiOperation(op)
            {
                Summary = "Get Random Weather forecast",
                OperationId = "GetWeatherForecast",
            })
            .Produces<WeatherForecast[]>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .HasApiVersion(1)
            .WithTags("Weather");
        return app;

    }
    internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }

    private static void ConfigureSwagger(SwaggerOptions options)
    {
        options.PreSerializeFilters.Add(static (swaggerDoc, httpReq) =>
        {
            swaggerDoc.Servers = new List<OpenApiServer>
            {
                new () { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" }
            };
        });
    }

    private static void ConfigureSwaggerUI(SwaggerUIOptions options, IEnumerable<ApiVersionDescription> apiVersions)
    {
        foreach (ApiVersionDescription description in apiVersions)
        {
            options.SwaggerEndpoint($"/swagger/v1/swagger.json", description.GroupName);
        }
    }
}

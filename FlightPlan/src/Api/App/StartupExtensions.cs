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

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using FlightPlan.Api.App.Authentication;
using FlightPlan.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using TSMoreland.Text.Json.NamingStrategies;
using TSMoreland.Text.Json.NamingStrategies.Strategies;

namespace FlightPlan.Api.App;

public static class StartupExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                JsonSerializerOptions serializeOptions = options.JsonSerializerOptions;
                serializeOptions.PropertyNamingPolicy = JsonStrategizedNamingPolicy.SnakeCase;
                serializeOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                serializeOptions.Converters
                    .Add(new JsonStrategizedStringEnumConverterFactory(new SnakeCaseEnumNamingStrategy()));

            });

        builder.Services
            .AddTransient<IUserService, UserService>()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc("flightPlan", new OpenApiInfo
                {
                    Title = "Flight Plan API",
                    Version = "v3",
                    Description = "Flight Plan Demo REST API",
                });
                options.AddSecurityDefinition("basicAuth", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using bearer scheme",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basicAuth"
                            },
                        },
                        Array.Empty<string>()
                    },
                });
                options.EnableAnnotations();
                string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            })
            .AddCors()
            .AddPersistence(builder.Configuration);

        builder.Services
            .AddAuthentication("BasicAuthentiation")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentiation", null);

        return builder;
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
                options.SwaggerEndpoint("/swagger/flightplan/swagger.json", "Flight Plan API"));
        }

        app.UseCors(config =>
            config
                .AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowAnyMethod());


        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}

using System.Reflection;
using System.Text.Json.Serialization;
using FlightPlan.Api.App.Authentication;
using FlightPlan.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using TSMoreland.Text.Json.NamingStrategies;
using TSMoreland.Text.Json.NamingStrategies.Strategies;

namespace FlightPlan.Api.App;

internal static class StartupExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                var serializeOptions = options.JsonSerializerOptions;
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
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            })
            .AddCors()
            .AddPersistence(builder.Configuration);

        builder.Services
            .AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentiation", null);

        // TODO: Use AddAuthorizationBuilder to register authorization services and construct policies
        builder.Services.AddAuthorization(options =>
        {
        });

        return builder;
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
                options.SwaggerEndpoint("/swagger/flightPlan/swagger.json", "Flight Plan API"));
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

using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CarInventory.App.Configuration;

public sealed class SwaggerConfiguration : ConfigureNamedOptions<SwaggerGenOptions, IApiVersionDescriptionProvider>
{
    /// <inheritdoc />
    public SwaggerConfiguration(IApiVersionDescriptionProvider dependency, Action<SwaggerGenOptions, IApiVersionDescriptionProvider>? action)
        : base(Options.DefaultName, dependency, action)
    {
    }

    /// <inheritdoc />
    public SwaggerConfiguration(IApiVersionDescriptionProvider dependency)
        : base(Options.DefaultName, dependency, SetupSwaggerDoc)
    {
    }

    private static void SetupSwaggerDoc(SwaggerGenOptions options, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
        foreach (ApiVersionDescription description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Version = description.ApiVersion.ToString(),
                Title = $"Cars Inventory v{description.ApiVersion}",
            });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization using the Bearer scheme",
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        }
    }
}

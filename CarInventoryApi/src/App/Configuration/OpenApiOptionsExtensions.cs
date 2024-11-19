using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace CarInventory.App.Configuration;

internal static class OpenApiOptionsExtensions
{
    public static OpenApiOptions UseJwtBearerAuthentication(this OpenApiOptions options)
    {
        OpenApiSecurityScheme scheme = new()
        {
            Type = SecuritySchemeType.Http,
            Name = JwtBearerDefaults.AuthenticationScheme,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Reference = new OpenApiReference()
            {
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme,
            },
        };

        options.AddDocumentTransformer((doc, _, _) =>
        {
            doc.Components ??= new OpenApiComponents();
            doc.Components.SecuritySchemes.Add(JwtBearerDefaults.AuthenticationScheme, scheme);
            return Task.CompletedTask;
        });
        options.AddOperationTransformer((operation, context, _) =>
        {
            if (context.Description.ActionDescriptor.EndpointMetadata.OfType<IAuthorizeData>().Any()) 
                operation.Security = [new OpenApiSecurityRequirement() { [scheme] = [] }];
            return Task.CompletedTask;
        });
        return options;
    }

    public static OpenApiOptions UseDocumentInfo(this OpenApiOptions options, string title, string version)
    {
        options.AddDocumentTransformer((doc, _, _) =>
        {
            doc.Info.Title = title;
            doc.Info.Version = version;
            return Task.CompletedTask;
        });
        return options;
    }

    public static OpenApiOptions UseServerUrls(this OpenApiOptions options, params string[] serverUrls)
    {
        options.AddDocumentTransformer((doc, _, _) =>
        {
            doc.Components ??= new OpenApiComponents();
            doc.Servers ??= [];

            doc.Servers.Clear();
            foreach (var serverUrl in serverUrls)
            {
                doc.Servers.Add(new OpenApiServer() { Url = serverUrl });
            }
            return Task.CompletedTask;
        });

        return options;
    }

}

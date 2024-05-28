using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;
using MicroShop.Services.ShoppingCart.ApiApp.Models.DataTransferObjects;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MicroShop.Services.ShoppingCart.ApiApp.Api;

public class StatusCodeWithResponseResult(HttpStatusCode statusCode, ResponseDto value) : IResult, IEndpointMetadataProvider, IStatusCodeHttpResult
{
    public ResponseDto Value { get; } = value;

    /// <inheritdoc />
    public Task ExecuteAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        httpContext.Response.StatusCode = StatusCode!.Value;
        JsonSerializerOptions jsonSerializerOptions = (httpContext.RequestServices.GetService<IOptions<JsonOptions>>()?.Value ?? new JsonOptions()).JsonSerializerOptions;
        return httpContext.Response.WriteAsJsonAsync<object>(
           Value,
           jsonSerializerOptions,
           contentType: MediaTypeNames.Application.Json);
    }

    /// <inheritdoc />
    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(method);
        ArgumentNullException.ThrowIfNull(builder);

        builder.Metadata.Add(new ProducesResponseTypeMetadata(StatusCodes.Status204NoContent, typeof(ResponseDto)));
    }

    /// <inheritdoc />
    public int? StatusCode { get; } = (int)statusCode;
}

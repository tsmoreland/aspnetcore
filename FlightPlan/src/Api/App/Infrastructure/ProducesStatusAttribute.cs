using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace FlightPlan.Api.App.Infrastructure;

/// <summary>
/// A filter that specifies the expected <see cref="System.Type"/> the action will return and the supported
/// response content types. The <see cref="ContentTypes"/> value is used to set
/// <see cref="ObjectResult.ContentTypes"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class ProducesStatusAttribute : Attribute, IResultFilter, IOrderedFilter, IApiResponseMetadataProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="statusCode"></param>
    /// <param name="contentType"></param>
    /// <param name="additionalContentTypes"></param>
    public ProducesStatusAttribute(int statusCode, string contentType, params string[] additionalContentTypes)
        : this(statusCode, null, contentType, additionalContentTypes)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="statusCode"></param>
    /// <param name="contentType"></param>
    /// <param name="type"></param>
    /// <param name="additionalContentTypes"></param>
    public ProducesStatusAttribute(int statusCode, Type? type, string contentType, params string[] additionalContentTypes)
    {
        ArgumentNullException.ThrowIfNull(contentType);

        Type = type;
        StatusCode = statusCode;
        MediaTypeHeaderValue.Parse(contentType);
        foreach (var additionalContentType in additionalContentTypes) MediaTypeHeaderValue.Parse(additionalContentType);
        ContentTypes = GetContentTypes(contentType, additionalContentTypes);
    }

    /// <inheritdoc />
    public void OnResultExecuting(ResultExecutingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (context.Result is ObjectResult objectResult) SetContentTypes(objectResult.ContentTypes);
    }

    /// <inheritdoc />
    public void OnResultExecuted(ResultExecutedContext context)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Type? Type { get; set; }

    /// <summary>
    /// Gets or sets the supported response content types. Used to set <see cref="ObjectResult.ContentTypes"/>.
    /// </summary>
    public MediaTypeCollection ContentTypes { get; set; } = new();

    /// <inheritdoc />
    public int StatusCode { get; private init; }

    /// <inheritdoc />
    public int Order { get; set; }

    /// <inheritdoc />
    public void SetContentTypes(MediaTypeCollection contentTypes)
    {
        contentTypes.Clear();
        foreach (var contentType in ContentTypes)
        {
            contentTypes.Add(contentType);
        }
    }

    private static MediaTypeCollection GetContentTypes(string firstArg, string[] args)
    {
        List<string> completeArgs = [ firstArg ];
        completeArgs.AddRange(args);
        var contentTypes = new MediaTypeCollection();
        foreach (var arg in completeArgs)
        {
            var contentType = new MediaType(arg);
            if (contentType.HasWildcard) throw new InvalidOperationException("Format match all  content/type is not allowed");
            contentTypes.Add(arg);
        }

        return contentTypes;
    }
}
